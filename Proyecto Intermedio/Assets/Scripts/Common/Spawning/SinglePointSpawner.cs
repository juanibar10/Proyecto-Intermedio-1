using UnityEngine;

public class SinglePointSpawner : MonoBehaviour, ISpawner
{
    public enum SpawnMode { Random, Fixed }

    public GameObject[] prefabs;

    [Header("Generation Mode")]
    public SpawnMode mode = SpawnMode.Random;

    [Header("Random configuration")]
    public float minY = -2f;
    public float maxY = 2f;

    [Header("Fixed configuration")]
    public float fixedY = 0f;

    public void Spawn(Transform origin)
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        float yValue;

        switch (mode)
        {
            case SpawnMode.Fixed:
                yValue = fixedY;
                break;

            default:
                yValue = Random.Range(minY, maxY);
                break;
        }

        Vector3 spawnPos = new Vector3(
            origin.position.x,
            yValue,
            origin.position.z
        );

        Object.Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
