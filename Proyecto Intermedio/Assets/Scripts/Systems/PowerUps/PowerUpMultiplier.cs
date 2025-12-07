public class PowerUpMultiplier : PowerUpBase
{
    public float duration = 10f;

    protected override void ActivatePowerUp(PlayerCollector player)
    {
        player.ActivateMultiplier(2, duration);
    }
}
