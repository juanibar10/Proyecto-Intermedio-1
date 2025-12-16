public class CollectibleManager : PoolManager<CollectibleBase>
{
    protected override void OnEnable()
    {
        GameEvents.OnCollectibleReturnToPool += Despawn;
    }

    protected override void OnDisable()
    {
        GameEvents.OnCollectibleReturnToPool -= Despawn;
    }
}