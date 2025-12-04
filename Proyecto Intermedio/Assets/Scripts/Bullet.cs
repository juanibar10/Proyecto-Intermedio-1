using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
   //1 derecha -1 izquierda asi se puede reusar en cualquier objecto
    public int direction = 1;

    private Rigidbody2D rb;
    private Renderer bulletRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletRenderer = GetComponent<Renderer>();
    }

    void Start()
    {
        rb.linearVelocity = new Vector2(direction * speed, 0);
    }

    void Update()
    {
        if (bulletRenderer != null && !bulletRenderer.isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(1);
            Debug.Log("Hit");
        }
        // Todo - verificar enemigos, paredes, etc.
        Destroy(gameObject);
    }
}
