using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Collectibles Pattern")]
    public GridSpawner collectibleSpawner;
    public float collectibleInterval = 2.5f;

    [Header("Enemies Pattern")]
    public SinglePointSpawner enemySpawner;
    public float enemyInterval = 3.5f;

    [Header("Power-Ups Pattern")]
    public SinglePointSpawner powerupSpawner;
    public float powerupInterval = 10f;

    [Header("Obstacles Pattern")]
    public SinglePointSpawner obstaclesSpawner;
    public float obstaclesInterval = 2f;

    private float collectibleTimer;
    private float enemyTimer;
    private float powerupTimer;
    private float obstaclesTimer;

    void Update()
    {
        float dt = Time.deltaTime;

        // COLLECTIBLES
        collectibleTimer += dt;
        if (collectibleTimer >= collectibleInterval)
        {
            collectibleSpawner.Spawn(transform);
            collectibleTimer = 0;
        }

        // ENEMIES
        enemyTimer += dt;
        if (enemyTimer >= enemyInterval)
        {
            enemySpawner.Spawn(transform);
            enemyTimer = 0;
        }

        // POWER-UPS
        powerupTimer += dt;
        if (powerupTimer >= powerupInterval)
        {
            powerupSpawner.Spawn(transform);
            powerupTimer = 0;
        }

        // POWER-UPS
        obstaclesTimer += dt;
        if (obstaclesTimer >= obstaclesInterval)
        {
            obstaclesSpawner.Spawn(transform);
            obstaclesTimer = 0;
        }
    }
}
