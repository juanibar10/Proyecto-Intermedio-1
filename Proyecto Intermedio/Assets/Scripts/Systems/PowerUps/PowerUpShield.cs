public class PowerUpShield : PowerUpBase
{
    public float duration = 10f;
    
    protected override void ActivatePowerUp(PlayerCollector player)
    {
        player.ActivateShield(duration);
    }
}
