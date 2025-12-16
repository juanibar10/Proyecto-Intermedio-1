public class PowerUpMagnet : PowerUpBase
{
    protected override void ActivatePowerUp(PlayerCollector player)
    {
        player.ActivateMagnet(Data.duration);
    }
}
