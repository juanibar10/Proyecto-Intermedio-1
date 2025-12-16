public class PowerUpManager : PoolManager<PowerUpBase>
{
    protected override void OnEnable()
    {
        GameEvents.OnPowerUpReturnToPool += Despawn;
    }

    protected override void OnDisable()
    {
        GameEvents.OnPowerUpReturnToPool -= Despawn;
    }
}