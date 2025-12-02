using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a basic interface for object pooling.
/// </summary>
/// <typeparam name="T">Type of the pooled object.</typeparam>
public interface IPool<in T> where T : MonoBehaviour
{
    void ReturnToPool(T obj);
}

/// <summary>
/// Represents an entry defining a prefab and its initial pool size.
/// </summary>
[Serializable]
public class PoolEntry<T> where T : MonoBehaviour
{
    public T prefab;
    public int initialSize = 20;
}

/// <summary>
/// Generic and fully reusable object pooling system.
/// Handles pre-instantiation, expansion, and tracking of objects by ID.
/// </summary>
public sealed class ObjectPool<T> : IPool<T> where T : MonoBehaviour
{
    private readonly Transform _parent;                            // Parent transform for organized hierarchy
    private readonly Dictionary<int, Queue<T>> _pools = new();     // Active queues per prefab ID
    private readonly Dictionary<int, T> _prefabs = new();          // Prefab references by ID
    private readonly Dictionary<T, int> _instanceToId = new();     // Reverse lookup from instance to ID
    private readonly Action<T> _onInstantiate;                     // Callback invoked on new instance creation
    private readonly Func<T, int> _getIdFunc;                      // Delegate to obtain a unique prefab ID
    private int _defaultID = -1;                                   // Optional fallback ID for default pool


    /// <summary>
    /// Initializes the pool using a list of entries and a function to derive IDs from prefabs.
    /// </summary>
    public ObjectPool(
        List<PoolEntry<T>> entries,
        Func<T, int> getIdFunc,
        Transform parent = null,
        Action<T> onInstantiate = null)
    {
        _parent = parent;
        _onInstantiate = onInstantiate;
        _getIdFunc = getIdFunc ?? throw new ArgumentNullException(nameof(getIdFunc));
        InitializeFromEntries(entries);
    }

    /// <summary>
    /// Creates internal structures and preloads objects.
    /// </summary>
    private void Initialize(Dictionary<int, T> prefabMap, int initialSize)
    {
        foreach (var (id, prefab) in prefabMap)
        {
            if (prefab == null)
            {
                Debug.LogWarning($"ObjectPool<{typeof(T).Name}>: Missing prefab for ID {id}, skipped.");
                continue;
            }

            if (!_prefabs.TryAdd(id, prefab))
            {
                Debug.LogWarning($"ObjectPool<{typeof(T).Name}>: Duplicate ID {id}, ignored.");
                continue;
            }

            _pools[id] = new Queue<T>();

            if (initialSize > 0)
                Expand(id, initialSize);

            _defaultID = _defaultID == -1 ? id : _defaultID;
        }
    }

    /// <summary>
    /// Initializes pool data from serialized entries.
    /// </summary>
    private void InitializeFromEntries(List<PoolEntry<T>> entries)
    {
        var prefabMap = new Dictionary<int, T>();
        var initialSizes = new Dictionary<int, int>();

        foreach (var entry in entries)
        {
            if (entry.prefab == null)
            {
                Debug.LogWarning($"ObjectPool<{typeof(T).Name}>: Entry with missing prefab skipped.");
                continue;
            }

            var id = _getIdFunc(entry.prefab);
            if (prefabMap.ContainsKey(id))
            {
                Debug.LogWarning($"ObjectPool<{typeof(T).Name}>: Duplicate ID {id} ignored.");
                continue;
            }

            prefabMap[id] = entry.prefab;
            initialSizes[id] = Mathf.Max(1, entry.initialSize);
        }

        Initialize(prefabMap, 0);

        // Expand each pool according to configured sizes
        foreach (var (id, size) in initialSizes)
        {
            Expand(id, size);
        }
    }

    /// <summary>
    /// Expands the pool for the given prefab ID by instantiating additional objects.
    /// </summary>
    private void Expand(int id, int amount)
    {
        if (!_prefabs.TryGetValue(id, out var prefab))
        {
            Debug.LogError($"ObjectPool<{typeof(T).Name}>: No prefab found for ID {id}.");
            return;
        }

        if (!_pools.TryGetValue(id, out var queue))
        {
            queue = new Queue<T>();
            _pools[id] = queue;
        }

        for (var i = 0; i < amount; i++)
        {
            var obj = UnityEngine.Object.Instantiate(prefab, _parent);
            obj.gameObject.SetActive(false);
            _onInstantiate?.Invoke(obj);
            queue.Enqueue(obj);
            _instanceToId[obj] = id;
        }
    }

    /// <summary>
    /// Retrieves an available object from the pool for the specified ID.
    /// Expands the pool if no inactive objects remain.
    /// </summary>
    public T Get(int id)
    {
        if (!_pools.TryGetValue(id, out var queue))
        {
            Debug.LogError($"ObjectPool<{typeof(T).Name}>: No pool found for ID {id}");
            return null;
        }

        if (!_prefabs.ContainsKey(id))
        {
            Debug.LogError($"ObjectPool<{typeof(T).Name}>: No prefab found for ID {id}");
            return null;
        }

        if (queue.Count == 0)
            Expand(id, 1);

        var obj = queue.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Returns the given object into its respective pool.
    /// </summary>
    public void ReturnToPool(T obj)
    {
        if (!obj) return;

        obj.gameObject.SetActive(false);
        obj.transform.localPosition = Vector3.zero;

        if (_instanceToId.TryGetValue(obj, out var id) && _pools.TryGetValue(id, out var queue))
        {
            queue.Enqueue(obj);
        }
        else
        {
            Debug.LogWarning($"ObjectPool<{typeof(T).Name}>: Tried to return an untracked object, destroying instance.");
            UnityEngine.Object.Destroy(obj.gameObject);
        }
    }
}
