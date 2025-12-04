using System;
using UnityEngine;

public enum MovementType
{
    Background,
    Item
}
public class MovementManager : MonoBehaviour
{
   public MovementType movementType;

   private void Update()
   {
       if (movementType == MovementType.Background)
       {
           transform.position -= new Vector3(EnviromentalSpeedManager.Instance.backgroundSpeed, 0, 0) * Time.deltaTime;
       }
       else if(movementType == MovementType.Item)
       {
           transform.position -= new Vector3(EnviromentalSpeedManager.Instance.ItemSpeed, 0, 0) * Time.deltaTime;
       }
   }
}

