using UnityEngine;

public interface IPoolData
{
    int Id { get; }
}

public interface IDataProvider<out TData>
{
    TData Data { get; }
}


public abstract class DataPoolBehaviour<T, TData> 
    : BasePoolBehaviour<T>
    where T : MonoBehaviour
    where TData : IPoolData
{
    protected override int GetIdFromPrefab(T prefab)
    {
        if (!prefab.TryGetComponent<IDataProvider<TData>>(out var provider))
        {
            Debug.LogError(
                $"Prefab '{prefab.name}' does not implement IDataProvider<{typeof(TData).Name}>",
                prefab
            );
            return -1;
        }

        if (provider.Data == null)
        {
            Debug.LogError(
                $"Prefab '{prefab.name}' has no Data assigned ({typeof(TData).Name})",
                prefab
            );
            return -1;
        }

        return provider.Data.Id;
    }

}