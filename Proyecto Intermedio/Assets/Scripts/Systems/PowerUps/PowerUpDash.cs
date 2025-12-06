public class CollectibleDash : PowerUpBase
{
    public float speedBoost = 8f;
    public float duration = 1f;

    protected override void ActivatePowerUp(PlayerCollector player)
    {
        player.ActivateDash(speedBoost, duration);
    }
}
