using UnityEngine;

public class CollectibleBase : MonoBehaviour
{
    private bool isBeingPulled = false;
    private Transform pullTarget;

    private const float pullSpeed = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        ICollector collector = other.GetComponent<ICollector>();

        if (collector != null)
        {
            collector.Collect();
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
        //TODO-Utilizar el destroy global
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
