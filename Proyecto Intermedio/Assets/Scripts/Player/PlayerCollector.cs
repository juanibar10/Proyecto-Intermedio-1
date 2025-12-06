using UnityEngine;
using System.Collections;

public class PlayerCollector : MonoBehaviour
{
    private float magnetRadius = 10f;
    private bool magnetActive = false;
    private float magnetTimer = 0f;

    public void ActivateMagnet(float duration)
    {
        magnetActive = true;
        magnetTimer = duration;
    }

    void Update()
    {
        if (!magnetActive) return;

        magnetTimer -= Time.deltaTime;
        if (magnetTimer <= 0)
        {
            magnetActive = false;
        }

        PullCollectibles();
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

    public void ActivateDash(float boost, float duration)
    {
        StartCoroutine(DashRoutine(boost, duration));
    }

    private IEnumerator DashRoutine(float boost, float duration)
    {
        PlayerMovement move = GetComponent<PlayerMovement>();

        //move.speed += boost;

        yield return new WaitForSeconds(duration);

        //move.speed -= boost;
    }
}
