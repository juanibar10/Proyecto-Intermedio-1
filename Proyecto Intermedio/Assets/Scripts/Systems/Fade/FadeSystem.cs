using UnityEngine;
using DG.Tweening;

public class FadeSystem : Singleton<FadeSystem>
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float defaultFadeDuration = 0.35f;
    [SerializeField] private float defaultDelay = 0.5f;

    private Tween activeTween;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1f;
    }

    public Sequence FadeIn(float duration = -1f, float delay = -1f)
    {
        if (Mathf.Approximately(canvasGroup.alpha, 1f))
            return DOTween.Sequence();

        if (activeTween != null && activeTween.IsActive())
            activeTween.Kill();

        if (duration < 0f) duration = defaultFadeDuration;
        if (delay < 0f) delay = defaultDelay;

        var seq = DOTween.Sequence();

        seq.Append(canvasGroup.DOFade(1f, duration).SetUpdate(true));

        if (delay > 0f)
            seq.AppendInterval(delay);

        activeTween = seq;
        return seq;
    }

    public Sequence FadeOut(float duration = -1f, float delay = -1f)
    {
        if (Mathf.Approximately(canvasGroup.alpha, 0f))
            return DOTween.Sequence();

        if (activeTween != null && activeTween.IsActive())
            activeTween.Kill();

        if (duration < 0f) duration = defaultFadeDuration;
        if (delay < 0f) delay = defaultDelay;

        var seq = DOTween.Sequence();

        if (delay > 0f)
            seq.AppendInterval(delay);

        seq.Append(canvasGroup.DOFade(0f, duration).SetUpdate(true));

        activeTween = seq;
        return seq;
    }
}