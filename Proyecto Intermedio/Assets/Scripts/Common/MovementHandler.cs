using UnityEngine;

public enum ElementType
{
    Background,
    Item
}
public class MovementHandler : MonoBehaviour
{
   public ElementType elementType;
   public bool active = true;

   private void Update()
   {
       if (!active) return;
       
       var speed = elementType switch
       {
           ElementType.Background => EnvironmentSpeedManager.Instance.backgroundSpeed,
           ElementType.Item => EnvironmentSpeedManager.Instance.itemSpeed,
           _ => EnvironmentSpeedManager.Instance.backgroundSpeed
       };

       transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;
   }
}