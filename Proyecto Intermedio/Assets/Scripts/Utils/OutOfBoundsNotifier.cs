using UnityEngine;

/// <summary>
/// Interface for objects that can handle their own out-of-bounds behavior.
/// </summary>
public interface IOutOfBoundsHandler
{
    /// <summary>
    /// Returns the object to its respective pool when it goes out of bounds.
    /// </summary>
    void ReturnToPool();
}

public class OutOfBoundsNotifier : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private float leftOffset = 2f;

    private Camera _cam;
    private IOutOfBoundsHandler _handler;

    private void Awake()
    {
        _cam = FindAnyObjectByType<Camera>();
        _handler = GetComponent<IOutOfBoundsHandler>();
    }

    private void Update()
    {
        if (IsOutLeft())
            _handler?.ReturnToPool();
    }

    private bool IsOutLeft()
    {
        // Left boundary of orthographic camera
        var camHalfHeight = _cam.orthographicSize;
        var camHalfWidth = camHalfHeight * _cam.aspect;

        var worldLeft = _cam.transform.position.x - camHalfWidth;

        // The object is out of bounds only if it fully crosses left boundary
        return endPoint.position.x < worldLeft - leftOffset;
    }
}