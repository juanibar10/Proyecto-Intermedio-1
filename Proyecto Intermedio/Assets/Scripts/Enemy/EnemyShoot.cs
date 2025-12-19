using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public EnemyData data;
    public Transform firePoint;

    [Header("Audio")]
    [SerializeField] private AudioClip shootSound;

    private float nextShootTimer = 0f;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        ResetRandomTimer();
    }

    void Update()
    {
        nextShootTimer -= Time.deltaTime;

        if (nextShootTimer <= 0f)
        {
            Shoot();
            ResetRandomTimer();
        }
    }

    void Shoot()
    {
        int dir = transform.position.x > firePoint.position.x ? -1 : 1;
        BulletHelper.ShootBullet(
            data.bulletPrefab,
            firePoint.position,
            dir,
            BulletOwner.Enemy
        );

        if (shootSound != null && audioSource != null)
            audioSource.PlayOneShot(shootSound);
    }

    void ResetRandomTimer()
    {
        nextShootTimer = Random.Range(data.minShootDelay, data.maxShootDelay);
    }
}
