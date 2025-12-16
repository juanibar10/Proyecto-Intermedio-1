using UnityEngine;

public class CollectibleBase : MonoBehaviour, IOutOfBoundsHandler, IDataProvider<CollectibleData>
{
    [SerializeField] private CollectibleData data;
    public CollectibleData Data => data;
    
    private bool isBeingPulled;
    private Transform pullTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var collector = other.GetComponent<ICollector>();
        if (collector == null) return;
        
        collector.Collect();
        ReturnToPool();
    }

    public void StartPull(Transform target)
    {
        pullTarget = target;
        isBeingPulled = true;
    }

    private void Update()
    {
        if (!isBeingPulled || pullTarget == null)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            pullTarget.position,
            Time.deltaTime * data.pullSpeed
        );
    }
    
    public void ReturnToPool()
    {
        GameEvents.RaiseCollectibleReturnToPool(this);
        isBeingPulled = false;
        pullTarget = null;
    }
}
