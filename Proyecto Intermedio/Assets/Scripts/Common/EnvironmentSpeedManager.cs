using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class EnvironmentSpeedManager : Singleton<EnvironmentSpeedManager>
{
    [Header("Speed Settings")]
    [SerializeField] private float startSpeed = 5f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float acceleration = 0.5f;

    [Header("Speed Multipliers (for layers)")]
    public float backgroundSpeedMultiplier = 0.75f;
    public float itemSpeedMultiplier = 1f;
    public float projectileSpeedMultiplier = 1.5f; // NEW

    [Header("Stopping Animation Settings")]
    [SerializeField] private float stopDuration = 1.2f;
    [SerializeField] private Ease stopEase = Ease.OutExpo;

    [Header("Resume Animation Settings")]
    [SerializeField] private float resumeDuration = 0.4f;
    [SerializeField] private Ease resumeEase = Ease.OutBack;

    private float _currentSpeed;
    private float _savedSpeedBeforeStop;

    private Tween _stopTween;
    private Tween _resumeTween;

    private bool _isStopped;

    public float BackgroundSpeed => _currentSpeed * backgroundSpeedMultiplier;
    public float ItemSpeed => _currentSpeed * itemSpeedMultiplier;
    public float ProjectileSpeed => _currentSpeed * projectileSpeedMultiplier; // NEW

    private void OnEnable()
    {
        GameEvents.OnEnvironmentStop += OnStop;
        GameEvents.OnEnvironmentResume += OnResume;
    }

    private void OnDisable()
    {
        GameEvents.OnEnvironmentStop -= OnStop;
        GameEvents.OnEnvironmentResume -= OnResume;
    }

    private void Start()
    {
        _currentSpeed = startSpeed;
    }

    private void Update()
    {
        if (_isStopped) return;
        if (_stopTween != null && _stopTween.IsActive()) return;
        if (_resumeTween != null && _resumeTween.IsActive()) return;

        if (_currentSpeed >= maxSpeed) return;

        _currentSpeed += acceleration * Time.deltaTime;
        if (_currentSpeed > maxSpeed)
            _currentSpeed = maxSpeed;
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
    public void OnResume()
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
}
