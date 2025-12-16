using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Scriptable Objects/Obstacle Data")]
public class ObstacleData : ScriptableObject, IPoolData
{
    [SerializeField] private int id;
    public int Id => id;

    [SerializeField] private int heightInLanes;
    public int HeightInLanes => heightInLanes;
}