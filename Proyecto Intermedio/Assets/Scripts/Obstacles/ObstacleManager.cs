using System.Collections.Generic;

public class ObstacleManager : PoolManager<ObstacleParent>
{
    public IReadOnlyList<ObstacleData> AvailableObstacleData => (pool as ObstaclesPool)?.EntriesData;

    protected override void OnEnable()
    {
        GameEvents.OnObstacleReturnToPool += Despawn;
    }

    protected override void OnDisable()
    {
        GameEvents.OnObstacleReturnToPool -= Despawn;
    }
}