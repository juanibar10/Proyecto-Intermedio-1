using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public abstract class BasePoolBehaviour<T> : MonoBehaviour, IPool<T> where T : MonoBehaviour
{
    [Header("Pool Configuration")]
    [SerializeField, Space] protected List<PoolEntry<T>> entries = new();
    public IReadOnlyList<PoolEntry<T>> Entries => entries;
    
    public event System.Action OnPoolInitialized;

    private ObjectPool<T> _objectPool;

    protected virtual void Start()
    {
        InitializePool(entries, GetIdFromPrefab, transform, OnInstantiate);
    }

    /// <summary>
    /// Initializes the object pool with the provided configuration and callbacks.
    /// </summary>
    private void InitializePool(List<PoolEntry<T>> poolEntries, System.Func<T, int> idSelector,
        Transform poolParent, System.Action<T> onInstantiate = null)
    {
        _objectPool = new ObjectPool<T>(poolEntries, idSelector, poolParent, onInstantiate);

        // Notify inheritors
        OnPoolReady();

        // Notify external listeners
        OnPoolInitialized?.Invoke();
    }

    /// <summary>
    /// Called when the pool has finished initializing (for inheritors).
    /// </summary>
    protected virtual void OnPoolReady() { }

    /// <summary>
    /// Called each time a new instance is created in the pool.
    /// </summary>
    protected virtual void OnInstantiate(T instance) { }

    /// <summary>
    /// Must return the unique identifier for a prefab type.
    /// </summary>
    protected abstract int GetIdFromPrefab(T prefab);

    /// <summary>
    /// Retrieves an available object from the pool using its identifier.
    /// </summary>
    public virtual T Get(int id) => _objectPool.Get(id);

    /// <summary>
    /// Returns an object back to the pool for reuse.
    /// </summary>
    public virtual void ReturnToPool(T obj) => _objectPool.ReturnToPool(obj);
}