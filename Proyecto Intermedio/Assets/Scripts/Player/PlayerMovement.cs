using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float ascendSpeed = 6f;

    private float bottomLimit;
    private float topLimit;

    private Rigidbody2D rb;
    private InputAction jumpAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpAction = InputSystem.actions.FindAction("Jump");

        float camSize = Camera.main.orthographicSize;
        float camY = Camera.main.transform.position.y;
        bottomLimit = camY - camSize;
        topLimit = camY + camSize;
    }

    void Update()
    {
        HandleVerticalInput();
        ClampPosition();
    }

    private void HandleVerticalInput()
    {
        if (jumpAction.IsPressed())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, ascendSpeed);
        }
        else
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.9f);
            }
        }
    }

    private void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, bottomLimit + 1f, topLimit - 1f);
        transform.position = pos;
    }
}
