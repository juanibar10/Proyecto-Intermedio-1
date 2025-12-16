using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Lane
{
    Ground = 0,
    Low    = 1,
    Mid    = 2,
    High   = 3
}

public class LaneState
{
    public bool blocked;
}

public class LaneSpawnDirector : MonoBehaviour
{
    [Header("Spawn X")]
    [SerializeField] private float spawnOffsetX = 2f;

    [Header("Lane Heights")]
    [SerializeField] private float groundY = -3f;
    [SerializeField] private float lowY    = -1f;
    [SerializeField] private float midY    = 1f;
    [SerializeField] private float highY   = 3f;

    [Header("Spawn Distance")]
    [SerializeField] private float minSpawnDistance = 2.5f;
    [SerializeField] private float maxSpawnDistance = 3.5f;

    [Header("Obstacle Settings")]
    [SerializeField, Range(0f, 1f)] private float obstacleChance = 1f;
    [SerializeField] private bool forceObstacleSpawn = true;

    [Header("Managers")]
    [SerializeField] private ObstacleManager obstacleManager;

    private readonly Dictionary<Lane, LaneState> _lanes = new();
    private Camera _cam;
    private Lane[] _laneValues;

    private float _distanceAccumulator;
    private float _nextSpawnDistance;

    // ---------------------------------------------------------------------
    // UNITY LIFECYCLE
    // ---------------------------------------------------------------------
    private void Awake()
    {
        _cam = Camera.main;
        _laneValues = (Lane[])Enum.GetValues(typeof(Lane));

        foreach (var lane in _laneValues)
            _lanes[lane] = new LaneState();
    }

    private void Start()
    {
        _nextSpawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
    }

    private void Update()
    {
        var speed = EnvironmentSpeedManager.Instance.ItemSpeed;
        if (speed <= 0f)
            return;

        _distanceAccumulator += speed * Time.deltaTime;

        if (_distanceAccumulator >= _nextSpawnDistance)
        {
            _distanceAccumulator = 0f;
            _nextSpawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
            SpawnTick();
        }
    }

    // ---------------------------------------------------------------------
    // SPAWN TICK
    // ---------------------------------------------------------------------
    private void SpawnTick()
    {
        ResetLanes();
        TrySpawnObstacle();
    }

    private void ResetLanes()
    {
        foreach (var lane in _laneValues)
            _lanes[lane].blocked = false;
    }

    // ---------------------------------------------------------------------
    // OBSTACLES
    // ---------------------------------------------------------------------
    private void TrySpawnObstacle()
    {
        if (obstacleManager == null)
            return;

        if (!forceObstacleSpawn && Random.value > obstacleChance)
            return;

        var data = PickObstacleDataFromManager();
        if (data == null)
            return;

        var height = Mathf.Clamp(data.HeightInLanes, 1, _laneValues.Length);

        // Check if obstacle fits (always starts at Ground)
        for (var i = 0; i < height; i++)
        {
            var lane = _laneValues[i];
            if (_lanes[lane].blocked)
                return;
        }

        // Mark lanes as blocked
        for (var i = 0; i < height; i++)
        {
            var lane = _laneValues[i];
            _lanes[lane].blocked = true;
        }

        // Spawn obstacle
        obstacleManager.Spawn(GetSpawnPosition(Lane.Ground), data.Id);
    }

    private ObstacleData PickObstacleDataFromManager()
    {
        var available = obstacleManager.AvailableObstacleData;
        if (available == null || available.Count == 0)
            return null;

        var index = Random.Range(0, available.Count);
        return available[index];
    }

    // ---------------------------------------------------------------------
    // UTIL
    // ---------------------------------------------------------------------
    private Vector3 GetSpawnPosition(Lane lane)
    {
        var camHalfWidth = _cam.orthographicSize * _cam.aspect;
        var x = _cam.transform.position.x + camHalfWidth + spawnOffsetX;
        var pos = new Vector3(x, GetLaneY(lane), 0f);

#if UNITY_EDITOR
        Debug.DrawLine(pos + Vector3.up * 0.5f, pos + Vector3.down * 0.5f, Color.green, 1f);
        Debug.DrawLine(pos + Vector3.left * 0.5f, pos + Vector3.right * 0.5f, Color.green, 1f);
#endif

        return pos;
    }

    private float GetLaneY(Lane lane)
    {
        return lane switch
        {
            Lane.Ground => groundY,
            Lane.Low    => lowY,
            Lane.Mid    => midY,
            Lane.High   => highY,
            _ => 0f
        };
    }

    // ---------------------------------------------------------------------
    // DEBUG
    // ---------------------------------------------------------------------
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (var y = -4f; y <= 4f; y += 2f)
            Gizmos.DrawLine(new Vector3(-20f, y, 0f), new Vector3(20f, y, 0f));
    }
}
