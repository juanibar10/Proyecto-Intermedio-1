using UnityEngine;


[CreateAssetMenu(fileName = "PowerUpData", menuName = "Scriptable Objects/PowerUp Data")]
public class PowerUpData : ScriptableObject, IPoolData
{
    [SerializeField] private int id;
    public int Id => id;
    
    public float duration = 15f;
}