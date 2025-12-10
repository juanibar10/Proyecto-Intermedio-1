using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Scriptable Objects/Obstacle Data")]
public class ObstacleData : ScriptableObject
{
    [Tooltip("Unique identifier for this obstacle type")]
    public int id;
}