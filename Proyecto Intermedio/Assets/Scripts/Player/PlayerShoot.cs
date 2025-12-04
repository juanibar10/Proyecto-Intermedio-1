using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 0.25f;

    private InputAction shootAction;
    private float cooldownTimer = 0f;

    void Awake()
    {
        shootAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (shootAction != null && shootAction.WasPressedThisFrame())
        {
            TryShoot();
        }
    }

    void TryShoot()
    {
        if (cooldownTimer > 0f) return;

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        cooldownTimer = shootCooldown;
    }
}
