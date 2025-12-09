using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IOutOfBoundsHandler
{
    void ReturnToPool();
}

public class OutOfBoundsNotifier : MonoBehaviour
{
    [SerializeField] private ElementType elementType = ElementType.Item;
    [SerializeField] private float leftOffset = 2f;

    private Camera _cam;
    private IOutOfBoundsHandler _handler;
    private Bounds _localBounds;
    private bool _hasLocalBounds;

    private void Awake()
    {
        _cam = Camera.main;
        _handler = GetComponent<IOutOfBoundsHandler>();
    }

    private void Start()
    {
        CacheLocalBounds();
    }

    private void Update()
    {
        if (!_hasLocalBounds)
            return;

        var worldBounds = LocalToWorldBounds(_localBounds, transform);

        if (IsOutLeft(worldBounds))
            _handler?.ReturnToPool();
    }

    // -------------------------------------------------------------------------
    // Bounds Cache (Local Space)
    // -------------------------------------------------------------------------

    private void CacheLocalBounds()
    {
        switch (elementType)
        {
            case ElementType.Background:
                CacheTilemapBounds();
                break;

            case ElementType.Item:
                CacheSpriteRendererBounds();
                break;
        }
    }

    private void CacheTilemapBounds()
    {
        var tilemap = GetComponentInChildren<Tilemap>();
        if (!tilemap)
            return;

        var worldCorners = GetBoundsCorners(tilemap.localBounds, tilemap.transform);
        var localCorners = worldCorners.Select(c => transform.InverseTransformPoint(c)).ToArray();

        _localBounds = Encapsulate(localCorners);
        _hasLocalBounds = true;
    }

    private void CacheSpriteRendererBounds()
    {
        var renderers = GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length == 0)
            return;

        var points = renderers
            .SelectMany(r => GetBoundsCorners(r.bounds))
            .Select(world => transform.InverseTransformPoint(world))
            .ToArray();

        _localBounds = Encapsulate(points);
        _hasLocalBounds = true;
    }

    // -------------------------------------------------------------------------
    // Bounds Utility Methods
    // -------------------------------------------------------------------------

    private static Vector3[] GetBoundsCorners(Bounds b, Transform t = null)
    {
        var min = b.min;
        var max = b.max;

        var corners = new Vector3[8]
        {
            new(min.x, min.y, min.z),
            new(max.x, min.y, min.z),
            new(min.x, max.y, min.z),
            new(max.x, max.y, min.z),
            new(min.x, min.y, max.z),
            new(max.x, min.y, max.z),
            new(min.x, max.y, max.z),
            new(max.x, max.y, max.z)
        };

        if (t == null)
            return corners;

        for (var i = 0; i < corners.Length; i++)
            corners[i] = t.TransformPoint(corners[i]);

        return corners;
    }

    private static Bounds Encapsulate(Vector3[] points)
    {
        var bounds = new Bounds(points[0], Vector3.zero);
        for (var i = 1; i < points.Length; i++)
            bounds.Encapsulate(points[i]);
        return bounds;
    }

    private static Bounds LocalToWorldBounds(Bounds localB, Transform root)
    {
        var min = localB.min;
        var max = localB.max;

        var corners = new[]
        {
            new Vector3(min.x, min.y, min.z),
            new Vector3(max.x, min.y, min.z),
            new Vector3(min.x, max.y, min.z),
            new Vector3(max.x, max.y, min.z),
            new Vector3(min.x, min.y, max.z),
            new Vector3(max.x, min.y, max.z),
            new Vector3(min.x, max.y, max.z),
            new Vector3(max.x, max.y, max.z)
        };

        var world = new Vector3[corners.Length];
        for (var i = 0; i < corners.Length; i++)
            world[i] = root.TransformPoint(corners[i]);

        return Encapsulate(world);
    }

    // -------------------------------------------------------------------------
    // Bounds Check
    // -------------------------------------------------------------------------

    private bool IsOutLeft(Bounds worldBounds)
    {
        var camHalfHeight = _cam.orthographicSize;
        var camHalfWidth = camHalfHeight * _cam.aspect;

        var worldLeft = _cam.transform.position.x - camHalfWidth;
        var rightX = worldBounds.max.x;

        return rightX < worldLeft - leftOffset;
    }
}