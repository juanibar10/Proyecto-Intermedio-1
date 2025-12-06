using UnityEngine;

[System.Serializable]
public class GridSpawner : MonoBehaviour, ISpawner
{
    public GameObject[] prefabs;

    [Header("Spawn area")]
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Grid Pattern")]
    public int minRows = 1;
    public int maxRows = 4;

    public int minColumns = 2;
    public int maxColumns = 5;

    public float hSpacing = 0.4f;
    public float vSpacing = 0.4f;

    public void Spawn(Transform origin)
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

        int rows = Random.Range(minRows, maxRows + 1);
        int columns = Random.Range(minColumns, maxColumns + 1);

        float startY = Random.Range(minY, maxY);

        float halfHeight = (rows - 1) * vSpacing * 0.5f;

        for (int r = 0; r < rows; r++)
        {
            int colsThisRow = columns - Random.Range(0, 2);

            for (int c = 0; c < colsThisRow; c++)
            {
                Vector3 spawnPos = new Vector3(
                    origin.position.x + (c * hSpacing),
                    startY + (r * vSpacing) - halfHeight,
                    0
                );

                Object.Instantiate(prefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
