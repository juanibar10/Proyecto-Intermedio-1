using UnityEngine;

public interface IObstaclePool : IPool<ObstacleParent> { }

public class ObstaclesPool : BasePoolBehaviour<ObstacleParent>, IObstaclePool
{
    /// <summary>
    /// Retrieves the unique ID from the prefab to identify pool entries
    /// </summary>
    protected override int GetIdFromPrefab(ObstacleParent prefab) => prefab.Data.id;
}
