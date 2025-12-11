using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private ObstaclesPool obstaclePool;
    [SerializeField] private Transform spawnPoint;

    public void SpawnObstacle(Vector3 spawnPos)
    {
        var entries = obstaclePool.Entries;
        var count = entries.Count;

        if (count == 0)
            return;

        var index = Random.Range(0, count);
        var id = entries[index].prefab.Data.id;

        var obstacle = obstaclePool.Get(id);
        obstacle.transform.position = spawnPos;
    }

    private void DespawnObstacle(ObstacleParent obstacle)
    {
        obstaclePool.ReturnToPool(obstacle);
    }

    private void OnEnable()
    {
        GameEvents.OnObstacleReturnToPool += DespawnObstacle;
    }

    private void OnDisable()
    {
        GameEvents.OnObstacleReturnToPool -= DespawnObstacle;
    }
}