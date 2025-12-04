using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float minShootDelay = 0.2f;   
    public float maxShootDelay = 3.5f;   

    private float nextShootTimer = 0f;

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
        //invierte la direccion de la bala detectando el lado
        int dir = transform.position.x > firePoint.position.x ? -1 : 1;
        BulletHelper.ShootBullet(bulletPrefab, firePoint.position, dir);
    }

    void ResetRandomTimer()
    {
        nextShootTimer = Random.Range(minShootDelay, maxShootDelay);
    }
}
