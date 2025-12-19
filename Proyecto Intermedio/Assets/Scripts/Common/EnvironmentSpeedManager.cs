using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class EnvironmentSpeedManager : Singleton<EnvironmentSpeedManager>
{
    [Header("Speed Settings")]
    [SerializeField] private float startSpeed = 5f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float acceleration = 0.5f;

    [Header("Real Speed (Infinite Scaling)")]
    [SerializeField] private float realAcceleration = 0.2f;
    
    [Header("Speed Multipliers (for layers)")]
    public float backgroundSpeedMultiplier = 0.75f;
    public float itemSpeedMultiplier = 1f;
    public float projectileSpeedMultiplier = 1.5f;

    [Header("Stopping Animation Settings")]
    [SerializeField] private float stopDuration = 1.2f;
    [SerializeField] private Ease stopEase = Ease.OutExpo;

    [Header("Resume Animation Settings")]
    [SerializeField] private float resumeDuration = 0.4f;
    [SerializeField] private Ease resumeEase = Ease.OutBack;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float minMusicPitch = 0.9f;
    [SerializeField] private float maxMusicPitch = 1.2f;
    
    private float _currentSpeed;
    private float _savedSpeedBeforeStop;
    private float realSpeed;

    private Tween _stopTween;
    private Tween _resumeTween;

    private bool _isStopped;

    public float GetStartSpeed() => startSpeed;
    public float GetMaxSpeed()   => maxSpeed;
    public float GetAcceleration() => acceleration;
    public float GetCurrentSpeed() => _currentSpeed;

    public float BackgroundSpeed => _currentSpeed * backgroundSpeedMultiplier;
    public float ItemSpeed       => _currentSpeed * itemSpeedMultiplier;
    public float ProjectileSpeed => _currentSpeed * projectileSpeedMultiplier;
    public float NormalizedSpeed01 => Mathf.InverseLerp(startSpeed, maxSpeed, _currentSpeed);
    
    private void OnEnable()
    {
        GameEvents.OnPlayerKilled += OnStop;
        GameEvents.OnPlayerRevived += OnResume;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerKilled -= OnStop;
        GameEvents.OnPlayerRevived -= OnResume;
    }

    private void Start()
    {
        _currentSpeed = startSpeed;
        realSpeed = startSpeed;

        if (musicSource != null && !musicSource.isPlaying)
            musicSource.Play();
    }

    private void Update()
    {
        // DISTANCE TRACKING
        realSpeed += realAcceleration * Time.deltaTime;
        float distanceThisFrame = realSpeed * Time.deltaTime;
        StatisticsSystem.Instance.AddDistance(distanceThisFrame);

        if (_isStopped) return;
        if (_stopTween != null && _stopTween.IsActive()) return;
        if (_resumeTween != null && _resumeTween.IsActive()) return;

        if (_currentSpeed >= maxSpeed) return;

        _currentSpeed += acceleration * Time.deltaTime;
        if (_currentSpeed > maxSpeed)
            _currentSpeed = maxSpeed;
        
        UpdateMusic();
    }

    // -------------------------------------------------------------------------
    // STOP ENVIRONMENT
    // -------------------------------------------------------------------------
    [Button]
    private void OnStop()
    {
        if (_isStopped)
            return;

        _isStopped = true;
        _resumeTween?.Kill();

        _savedSpeedBeforeStop = _currentSpeed;

        _stopTween?.Kill();

        _stopTween = DOTween.To(
            () => _currentSpeed,
            x => _currentSpeed = x,
            0f,
            stopDuration
        )
        .SetEase(stopEase)
        .SetUpdate(true)
        .OnComplete(() => _currentSpeed = 0f);
    }

    // -------------------------------------------------------------------------
    // RESUME ENVIRONMENT
    // -------------------------------------------------------------------------
    [Button]
    private void OnResume()
    {
        if (!_isStopped)
            return;

        _isStopped = false;

        _stopTween?.Kill();
        _resumeTween?.Kill();

        _resumeTween = DOTween.To(
            () => _currentSpeed,
            x => _currentSpeed = x,
            _savedSpeedBeforeStop,
            resumeDuration
        )
        .SetEase(resumeEase)
        .SetUpdate(true);
    }
    
    private void UpdateMusic()
    {
        if (musicSource == null) return;

        float t = NormalizedSpeed01;
        musicSource.pitch = Mathf.Lerp(minMusicPitch, maxMusicPitch, t);
    }
}
