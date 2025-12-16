using UnityEngine;

public abstract class PoolManager<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected BasePoolBehaviour<T> pool;

    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }

    public virtual void Spawn(Vector3 position, int id)
    {
        var element = pool.Get(id);
        element.transform.position = position;
    }

    protected virtual void Despawn(T element)
    {
        pool.ReturnToPool(element);
    }
}