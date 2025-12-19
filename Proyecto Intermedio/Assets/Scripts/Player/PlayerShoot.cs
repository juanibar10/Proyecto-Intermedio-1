using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Sprite bulletSprite;
    public Transform firePoint;
    public float shootCooldown = 0.25f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip shootSound;

    private InputAction shootAction;
    private float shootDelay = 0.25f;
    private float cooldownTimer = 0f;

    private Animator animator;
    private AudioSource audioSource;

    void Awake()
    {
        shootAction = InputSystem.actions.FindAction("Attack");
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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

        animator.SetTrigger("Attack");
        StartCoroutine(SpawnBullet());

        cooldownTimer = shootCooldown;
    }

    private IEnumerator SpawnBullet()
    {
        yield return new WaitForSeconds(shootDelay);

        Bullet bullet = BulletHelper.ShootBullet(bulletPrefab, firePoint.position, 1, BulletOwner.Player);
        bullet.SetSprite(bulletSprite);
        
        if (shootSound != null && audioSource != null)
            audioSource.PlayOneShot(shootSound);
    }
}
