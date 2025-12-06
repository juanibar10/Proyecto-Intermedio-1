public class CollectibleMagnet : PowerUpBase
{
    public float duration = 3f;

    protected override void ActivatePowerUp(PlayerCollector player)
    {
        player.ActivateMagnet(duration);
    }
}
