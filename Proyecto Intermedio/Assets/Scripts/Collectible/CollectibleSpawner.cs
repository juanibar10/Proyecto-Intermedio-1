using UnityEngine;

//Clase creada únicamente para la prueba de Score System
//TODO-Se eliminará a futuro
public class CollectibleSpawner : MonoBehaviour
{
    public GameObject[] collectiblePrefabs;

    [Header("Spawn timing")]
    public float spawnInterval = 2.5f;

    [Header("Spawn area")]
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Grid Pattern")]
    public int minRows = 1;
    public int maxRows = 4;

    public int minColumns = 2;
    public int maxColumns = 6;

    public float horizontalSpacing = 1f;
    public float verticalSpacing = 1f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnGridPattern();
            timer = 0f;
        }
    }

    void SpawnGridPattern()
    {
        int prefabIndex = Random.Range(0, collectiblePrefabs.Length);
        GameObject prefab = collectiblePrefabs[prefabIndex];

        int rows = Random.Range(minRows, maxRows + 1);
        int columns = Random.Range(minColumns, maxColumns + 1);

        float startY = Random.Range(minY, maxY);

        float halfHeight = (rows - 1) * verticalSpacing * 0.5f;

        for (int r = 0; r < rows; r++)
        {
            int colsThisRow = columns - Random.Range(0, 2);

            for (int c = 0; c < colsThisRow; c++)
            {
                Vector3 pos = new Vector3(
                    transform.position.x + (c * horizontalSpacing),
                    startY + (r * verticalSpacing) - halfHeight,
                    0
                );

                Instantiate(prefab, pos, Quaternion.identity);
            }
        }
    }
}
