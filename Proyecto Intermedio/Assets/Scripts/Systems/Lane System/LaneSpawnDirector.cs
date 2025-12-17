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

public enum LaneContent
{
    Empty,
    Obstacle,
    Enemy,
    Collectible,
    PowerUp
}

public class LaneState
{
    public LaneContent content = LaneContent.Empty;
}

public class LaneSpawnDirector : MonoBehaviour
{
    [Header("Spawn X")]
    [SerializeField] private float spawnOffsetX = 2f;

    [Header("Lane Layout")]
    [SerializeField] private float groundY = -3f;
    [SerializeField] private float laneHeight = 2f;

    [Header("Spawn Distance")]
    [SerializeField] private float minSpawnDistance = 2.5f;
    [SerializeField] private float maxSpawnDistance = 3.5f;

    [Header("Spawn Scaling")]
    [SerializeField] private float minSpawnDistanceAtMaxSpeed = 4.5f;
    [SerializeField] private float maxSpawnDistanceAtMaxSpeed = 6.5f;
    [SerializeField] private float minSpawnSpeedFactor = 0.6f;
    [SerializeField] private float maxSpawnSpeedFactor = 1f;

    [Header("Chances")]
    [SerializeField, Range(0f, 1f)] private float obstacleChance   = 0.6f;
    [SerializeField, Range(0f, 1f)] private float enemyChance      = 0.5f;
    [SerializeField, Range(0f, 1f)] private float collectibleChance = 0.6f;
    [SerializeField, Range(0f, 1f)] private float powerUpChance     = 0.15f;

    [Header("Managers")]
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private CollectibleManager collectibleManager;
    [SerializeField] private PowerUpManager powerUpManager;

    private readonly Dictionary<Lane, LaneState> _lanes = new();
    private Lane[] _laneValues;
    private Camera _cam;

    private float _distanceAccumulator;
    private float _nextSpawnDistance;

    private void Awake()
    {
        _cam = Camera.main;
        _laneValues = (Lane[])Enum.GetValues(typeof(Lane));

        foreach (var lane in _laneValues)
            _lanes[lane] = new LaneState();
    }

    private void Start()
    {
        ResetNextSpawnDistance();
    }

    private void Update()
    {
        var speedManager = EnvironmentSpeedManager.Instance;
        var speed = speedManager.ItemSpeed;
        if (speed <= 0f) return;

        var t = speedManager.NormalizedSpeed01;

        var spawnSpeedFactor = Mathf.Lerp(
            minSpawnSpeedFactor,
            maxSpawnSpeedFactor,
            t
        );

        _distanceAccumulator += speed * spawnSpeedFactor * Time.deltaTime;

        if (_distanceAccumulator < _nextSpawnDistance) return;

        _distanceAccumulator = 0f;
        ResetNextSpawnDistance();
        SpawnTick();
    }

    // ---------------------------------------------------------------------
    private void SpawnTick()
    {
        ResetLanes();

        TrySpawnObstacle();
        TrySpawnEnemy();
        TrySpawnCollectibles();
        TrySpawnPowerUp();
    }

    private void ResetLanes()
    {
        foreach (var lane in _laneValues)
            _lanes[lane].content = LaneContent.Empty;
    }

    // ---------------------------------------------------------------------
    private void TrySpawnObstacle()
    {
        if (!obstacleManager || Random.value > obstacleChance)
            return;

        var data = PickRandom(obstacleManager.AvailableObstacleData);
        if (!data) return;

        var height = Mathf.Clamp(data.HeightInLanes, 1, _laneValues.Length);

        for (var i = 0; i < height; i++)
            if (_lanes[_laneValues[i]].content != LaneContent.Empty)
                return;

        for (var i = 0; i < height; i++)
            _lanes[_laneValues[i]].content = LaneContent.Obstacle;

        obstacleManager.Spawn(GetSpawnPositionGround(), data.Id);
    }

    // ---------------------------------------------------------------------
    private void TrySpawnEnemy()
    {
        if (!enemyManager || Random.value > enemyChance)
            return;

        var data = PickRandom(enemyManager.AvailableEnemyData);
        if (!data) return;

        var validLanes = GetValidEnemyLanes(data.isFlyer);
        if (validLanes.Count == 0) return;

        var lane = validLanes[Random.Range(0, validLanes.Count)];
        enemyManager.Spawn(GetSpawnPositionForLane(lane), data.Id);

        _lanes[lane].content = LaneContent.Enemy;
    }

    private List<Lane> GetValidEnemyLanes(bool isFlyer)
    {
        var result = new List<Lane>();

        foreach (var lane in _laneValues)
        {
            if (_lanes[lane].content != LaneContent.Empty)
                continue;

            if (!isFlyer && lane == Lane.Ground)
                result.Add(lane);
            else if (isFlyer && (lane == Lane.Mid || lane == Lane.High))
                result.Add(lane);
        }

        return result;
    }

    // ---------------------------------------------------------------------
    private void TrySpawnCollectibles()
    {
        if (!collectibleManager) return;

        foreach (var lane in _laneValues)
        {
            if (_lanes[lane].content != LaneContent.Empty)
                continue;

            if (Random.value > collectibleChance)
                continue;

            collectibleManager.Spawn(GetSpawnPositionForLane(lane), 0);
            _lanes[lane].content = LaneContent.Collectible;
        }
    }

    // ---------------------------------------------------------------------
    private void TrySpawnPowerUp()
    {
        if (!powerUpManager || Random.value > powerUpChance)
            return;

        var freeLanes = GetFreeLanes();
        if (freeLanes.Count == 0) return;

        var lane = freeLanes[Random.Range(0, freeLanes.Count)];
        powerUpManager.Spawn(GetSpawnPositionForLane(lane), 0);

        _lanes[lane].content = LaneContent.PowerUp;
    }

    // ---------------------------------------------------------------------
    private Vector3 GetSpawnPositionGround()
        => new Vector3(GetSpawnX(), groundY, 0f);

    private Vector3 GetSpawnPositionForLane(Lane lane)
        => new Vector3(GetSpawnX(), groundY + (int)lane * laneHeight, 0f);

    private float GetSpawnX()
    {
        var camHalfWidth = _cam.orthographicSize * _cam.aspect;
        return _cam.transform.position.x + camHalfWidth + spawnOffsetX;
    }

    // ---------------------------------------------------------------------
    private static T PickRandom<T>(IReadOnlyList<T> list)
        => list == null || list.Count == 0 ? default : list[Random.Range(0, list.Count)];

    private List<Lane> GetFreeLanes()
    {
        var result = new List<Lane>();
        foreach (var lane in _laneValues)
            if (_lanes[lane].content == LaneContent.Empty)
                result.Add(lane);
        return result;
    }

    private void ResetNextSpawnDistance()
    {
        var t = EnvironmentSpeedManager.Instance.NormalizedSpeed01;

        var min = Mathf.Lerp(minSpawnDistance, minSpawnDistanceAtMaxSpeed, t);
        var max = Mathf.Lerp(maxSpawnDistance, maxSpawnDistanceAtMaxSpeed, t);

        _nextSpawnDistance = Random.Range(min, max);
    }
}
