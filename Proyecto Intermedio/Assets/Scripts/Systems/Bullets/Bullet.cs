using UnityEngine;

public class Bullet : MonoBehaviour
{
                         
    public int direction = 1; // 1 => Derecha,  -1 => Izquierda

    public BulletOwner owner;
    
    private Rigidbody2D rb;
    private Renderer bulletRenderer;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletRenderer = GetComponent<Renderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        //rb.linearVelocity = new Vector2(direction * speed, 0);
        FlipSprite();
    }

    void Update()
    {
        if (!bulletRenderer.isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        
        if (damageable == null) return;


        if (owner == BulletOwner.Player && collision.CompareTag("Enemy")) //TODO
        {
            damageable.TakeDamage(1, owner);
            Destroy(gameObject);
        }

        if (owner == BulletOwner.Enemy && collision.CompareTag("Player")) //TODO
        {
            damageable.TakeDamage(10, owner);
            Destroy(gameObject);
        }
    }
    
    private void FlipSprite()
    {
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * direction; // * 1 o -1
        transform.localScale = s;
    }

    public void SetSprite(Sprite newSprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }
}
