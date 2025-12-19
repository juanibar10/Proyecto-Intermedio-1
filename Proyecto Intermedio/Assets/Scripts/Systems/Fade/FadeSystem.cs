using UnityEngine;
using DG.Tweening;

public class FadeSystem : Singleton<FadeSystem>
{
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 0.35f;

    private Tween _currentTween;

    public Tween FadeIn()
    {
        _currentTween?.Kill();

        if (fadeGroup.alpha >= 0.999f)
        {
            // Ya está en fade in → tween dummy inmediato
            return DOVirtual.DelayedCall(0f, () => { })
                .SetUpdate(true);
        }

        fadeGroup.blocksRaycasts = true;
        fadeGroup.alpha = Mathf.Clamp01(fadeGroup.alpha);

        _currentTween = fadeGroup
            .DOFade(1f, fadeDuration)
            .SetUpdate(true);

        return _currentTween;
    }

    public Tween FadeOut()
    {
        _currentTween?.Kill();

        if (fadeGroup.alpha <= 0.001f)
        {
            fadeGroup.blocksRaycasts = false;

            return DOVirtual.DelayedCall(0f, () => { })
                .SetUpdate(true);
        }

        _currentTween = fadeGroup
            .DOFade(0f, fadeDuration)
            .SetUpdate(true)
            .OnComplete(() => fadeGroup.blocksRaycasts = false);

        return _currentTween;
    }
}