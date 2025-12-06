using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class PlayerDamageFeedback : MonoBehaviour
{
    private Animator animator;
    private Damageable damageable;
    private SpriteRenderer spriteRenderer;

    private Color originalColor;
    private Color flashColor = new Color(1f, 0.4f, 0.4f);

    private float flashTime = 0.15f;
    private int flashCount = 2;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        damageable.onDamageEvent.AddListener(OnDamageTaken);
    }

    private void OnDisable()
    {
        damageable.onDamageEvent.RemoveListener(OnDamageTaken);
    }

    private void OnDamageTaken()
    {
        // Activa animación
        animator.SetTrigger("Damage");

        // Flash visual
        StopAllCoroutines();
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashTime);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashTime);
        }
    }
}
