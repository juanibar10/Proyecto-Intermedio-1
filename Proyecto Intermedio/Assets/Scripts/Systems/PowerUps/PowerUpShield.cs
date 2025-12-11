public class PowerUpShield : PowerUpBase
{
    protected override void ActivatePowerUp(PlayerCollector player)
    {
        player.ActivateShield(duration);
    }
}
