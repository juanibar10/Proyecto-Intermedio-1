using System;

public static class GameEvents
{
    /// <summary>Triggered when a chunk is returned to the object pool.</summary>
    public static event Action<Chunk> OnChunkReturnToPool;
    public static void RaiseChunkReturnToPool(Chunk chunk) =>
        OnChunkReturnToPool?.Invoke(chunk);
    
    public static event Action<BaseEnemy> OnEnemyReturnToPool;
    public static void RaiseEnemyReturnToPool(BaseEnemy enemy) =>
        OnEnemyReturnToPool?.Invoke(enemy);
    
    public static event Action<PowerUpBase> OnPowerUpReturnToPool;
    public static void RaisePowerUpReturnToPool(PowerUpBase powerUp) =>
        OnPowerUpReturnToPool?.Invoke(powerUp);
    
    
    public static event Action OnPlayerKilled;
    public static void RaisePlayerKilled() =>
        OnPlayerKilled?.Invoke();
    
    
    public static event Action OnPlayerRevived;
    public static void RaisePlayerRevived() =>
        OnPlayerRevived?.Invoke();

    public static event Action<ObstacleParent> OnObstacleReturnToPool;
    public static void RaiseObstacleReturnToPool(ObstacleParent obstacleParent) => 
        OnObstacleReturnToPool?.Invoke(obstacleParent);
    
    
    public static event Action<CollectibleBase> OnCollectibleReturnToPool;
    public static void RaiseCollectibleReturnToPool(CollectibleBase collectible) => 
        OnCollectibleReturnToPool?.Invoke(collectible);

}
