using System.Collections.Generic;
using UnityEngine;

public class ObstacleParent : MonoBehaviour, IOutOfBoundsHandler, IDataProvider<ObstacleData>
{
    [SerializeField] private ObstacleData data;
    public ObstacleData Data => data;
    
    [SerializeField] private List<Obstacle> children = new();

    private int _childrenDestroyedCount = 0;

    private void OnEnable()
    {
        foreach (var child in children)
        {
            child.OnDestroyed += OnChildDestroyed;
        }
    }

    private void OnDisable()
    {
        foreach (var child in children)
        {
            child.OnDestroyed -= OnChildDestroyed;
        }
    }

    private void OnChildDestroyed()
    {
        _childrenDestroyedCount++;

        if (_childrenDestroyedCount >= children.Count)
        {
            ReturnToPool();
        }
    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        _childrenDestroyedCount = 0;
    }
    
    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        GameEvents.RaiseObstacleReturnToPool(this);
        _childrenDestroyedCount = 0;
        children.ForEach(child => child.RestoreObstacle());
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        var p = transform.position;
        var size = 0.25f;
        // Cross
        Gizmos.DrawLine(p + Vector3.up * size,    p + Vector3.down * size);
        Gizmos.DrawLine(p + Vector3.left * size,  p + Vector3.right * size);

        // Small sphere at pivot
        Gizmos.DrawSphere(p, size * 0.15f);
    }
}
