using UnityEngine;
using System.Collections;

public class PlayerCollector : MonoBehaviour
{
    private float magnetRadius = 10f;
    private bool magnetActive = false;
    private float magnetTimer = 0f;

    private bool shieldActive = false;
    private float shieldTimer = 0f;

    public float MagnetTimeLeft => magnetActive ? magnetTimer : 0f;
    public float ShieldTimeLeft => shieldActive ? shieldTimer : 0f;
    public bool IsMagnetActive => magnetActive;
    public bool IsShieldActive => shieldActive;

    public void ActivateMagnet(float duration)
    {
        magnetActive = true;
        magnetTimer = duration;
    }

    public void ActivateMultiplier(int multiplier, float duration)
    {
        StatisticsSystem.Instance.SetCollectibleMultiplier(multiplier, duration);
    }

    public void ActivateShield(float duration)
    {
        shieldActive = true;
        shieldTimer = duration;
    }

    void Update()
    {
        // MAGNET TIMER
        if (magnetActive)
        {
            magnetTimer -= Time.deltaTime;
            if (magnetTimer <= 0)
                magnetActive = false;

            PullCollectibles();
        }

        // SHIELD TIMER
        if (shieldActive)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                shieldActive = false;
            }
        }
    }

    private void PullCollectibles()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, magnetRadius);

        foreach (var hit in hits)
        {
            var collectible = hit.GetComponent<CollectibleBase>();
            if (collectible != null)
            {
                collectible.StartPull(transform);
            }
        }
    }
}
