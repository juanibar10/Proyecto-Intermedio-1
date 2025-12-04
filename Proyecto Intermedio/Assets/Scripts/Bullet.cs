using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody2D rb;
    private Renderer bulletRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletRenderer = GetComponent<Renderer>();
    }

    void Start()
    {
        rb.linearVelocity = transform.right * speed;
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
        // Todo - verificar enemigos, paredes, etc.
        Destroy(gameObject);
    }
}
