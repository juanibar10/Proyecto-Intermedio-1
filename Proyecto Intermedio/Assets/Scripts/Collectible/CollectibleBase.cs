using UnityEngine;

public class CollectibleBase : MonoBehaviour
{
    public int value = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();

        if (stats != null)
        {
            stats.AddCollectible(value);
            Destroy(gameObject);
        }
    }
}
