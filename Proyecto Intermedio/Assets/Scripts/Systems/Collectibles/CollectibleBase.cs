using UnityEngine;

public class CollectibleBase : MonoBehaviour
{
    public int value = 10;

    private bool isBeingPulled = false;
    private Transform pullTarget;

    private const float pullSpeed = 15f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        ICollector collector = other.GetComponent<ICollector>();

        if (collector != null)
        {
            collector.Collect(value);
            Destroy(gameObject);
        }
    }

    public void StartPull(Transform target)
    {
        pullTarget = target;
        isBeingPulled = true;
    }

    private void Update()
    {
        if (transform.position.x < -15f)
            Destroy(gameObject);
        
        if (!isBeingPulled || pullTarget == null)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            pullTarget.position,
            Time.deltaTime * pullSpeed
        );
    }
}
