using UnityEngine;

public enum ElementType
{
    Background,
    Item,
    Projectile
}
public class MovementHandler : MonoBehaviour
{
   public ElementType elementType;
   [HideInInspector] public bool active = true;
   [SerializeField] public bool invertSpeed;
   
   private void Update()
   {
       if (!active) return;
       
       var speed = elementType switch
       {
           ElementType.Background => EnvironmentSpeedManager.Instance.BackgroundSpeed,
           ElementType.Item => EnvironmentSpeedManager.Instance.ItemSpeed,
           ElementType.Projectile => EnvironmentSpeedManager.Instance.ProjectileSpeed,
           _ => EnvironmentSpeedManager.Instance.BackgroundSpeed
       };

       speed = invertSpeed ? -speed : speed;
       transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
   }
}