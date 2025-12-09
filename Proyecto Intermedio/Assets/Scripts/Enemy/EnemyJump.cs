using UnityEngine;

public class EnemyJump : MonoBehaviour
{
     [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float jumpInterval = 2f;
    
        private Rigidbody2D rb;
        private float timer;
    
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        
        void Start()
        {
            Collider2D enemyCol = GetComponent<Collider2D>();
            Collider2D playerCol = GameObject.FindWithTag("Player").GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(enemyCol, playerCol);
        }
    
        private void Update()
        {
            timer += Time.deltaTime;
    
            if (timer >= jumpInterval)
            {
                Jump();
                timer = 0f;
            }
        }
    
        private void Jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
}
