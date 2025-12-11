using UnityEngine;
using System;
using System.Collections;

public class StatisticsSystem : MonoBehaviour
{
    public static StatisticsSystem Instance { get; private set; }

    [Header("Score Settings")]
    [SerializeField] private float scorePerUnit = 5f;

    [Header("Multipliers")]
    [SerializeField] private int collectibleMultiplier = 1;
    [SerializeField] private int scoreMultiplier = 1;

    private float scoreAccumulator = 0f;

    public event Action OnStatsChanged;
    public RunStatistics CurrentRun { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ResetStats();
    }

    public void ResetStats()
    {
        CurrentRun = new RunStatistics();
        collectibleMultiplier = 1;
        scoreMultiplier = 1;
        scoreAccumulator = 0f;

        OnStatsChanged?.Invoke();
    }

    public void AddDistance(float distance)
    {
        CurrentRun.distanceTravelled += distance;

        float increment = distance * scorePerUnit * scoreMultiplier;
        scoreAccumulator += increment;

        while (scoreAccumulator >= 1f)
        {
            CurrentRun.score++;
            scoreAccumulator -= 1f;
        }
        
        OnStatsChanged?.Invoke();
    }

    public void AddCollectible()
    {
        CurrentRun.coinsCollected += collectibleMultiplier;
        OnStatsChanged?.Invoke();
    }

    public void AddKill()
    {
        CurrentRun.enemiesKilled++;
        OnStatsChanged?.Invoke();
    }

    public void SetCollectibleMultiplier(int mult, float duration)
    {
        StartCoroutine(CollectibleMultiplierRoutine(mult, duration));
    }

    private IEnumerator CollectibleMultiplierRoutine(int mult, float duration)
    {
        collectibleMultiplier = mult;
        yield return new WaitForSeconds(duration);
        collectibleMultiplier = 1;
    }
}
