public class PowerUpMultiplier : PowerUpBase
{
    protected override void ActivatePowerUp(PlayerCollector player)
    {
        player.ActivateMultiplier(2, Data.duration);
    }
}
