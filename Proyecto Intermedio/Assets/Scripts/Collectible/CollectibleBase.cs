using UnityEngine;

public class CollectibleBase : MonoBehaviour
{
    public int value = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        ICollector collector = other.GetComponent<ICollector>();

        if (collector != null)
        {
            collector.Collect(value);
            Destroy(gameObject);
        }
    }
}
