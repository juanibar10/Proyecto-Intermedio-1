using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class MeatWallAdvance : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [Range(0f, 1f)] 
    [SerializeField] private float healthThresholdRatio = 0.5f;

    [SerializeField] private Vector3 behindOffset = new Vector3(-1.6f, 0f, 0f);
    [SerializeField] private bool respectPlayerFacing = true;

    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private bool moveInstantly = false;
    [SerializeField] private bool triggerOnce = true;

    private Transform playerTransform;
    private Damageable playerDamageable;
    private bool activated = false;
    private Vector3 targetPosition;
    private Coroutine moveCoroutine;

    private void Start()
    {
        GameObject playerGO = GameObject.FindWithTag(playerTag);
        if (playerGO == null) return;

        playerTransform = playerGO.transform;
        playerDamageable = playerGO.GetComponent<Damageable>() 
                           ?? playerGO.GetComponentInParent<Damageable>() 
                           ?? playerGO.GetComponentInChildren<Damageable>();

        if (playerDamageable != null)
        {
            try { playerDamageable.OnHealthChanged += OnPlayerHealthChanged; }
            catch { }
        }
    }

    private void OnDestroy()
    {
        if (playerDamageable != null)
        {
            try { playerDamageable.OnHealthChanged -= OnPlayerHealthChanged; }
            catch { }
        }
    }

    private void OnPlayerHealthChanged(int current, int max)
    {
        float ratio = (float)current / Mathf.Max(1, max);
        TryActivate(ratio);
    }

    private void Update()
    {
        if (activated || playerDamageable == null) return;

        float ratio = (float)playerDamageable.Health / Mathf.Max(1, playerDamageable.MaxHealth);
        TryActivate(ratio);
    }

    private void TryActivate(float ratio)
    {
        if (activated && triggerOnce) return;

        if (ratio <= healthThresholdRatio)
        {
            activated = true;
            ComputeTargetPosition();

            if (moveInstantly)
            {
                transform.position = targetPosition;
            }
            else
            {
                if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                moveCoroutine = StartCoroutine(MoveToTarget());
            }
        }
    }

    private void ComputeTargetPosition()
    {
        if (playerTransform == null)
        {
            targetPosition = transform.position;
            return;
        }

        Vector3 offset = behindOffset;

        if (respectPlayerFacing)
        {
            float sign = Mathf.Sign(playerTransform.localScale.x);
            offset.x = Mathf.Abs(behindOffset.x) * sign * -1f;
        }

        targetPosition = playerTransform.TransformPoint(offset);
    }

    private IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        moveCoroutine = null;
    }
}
