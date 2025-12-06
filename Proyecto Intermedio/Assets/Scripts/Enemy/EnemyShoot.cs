using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public EnemyData data;
    public Transform firePoint;

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
        BulletHelper.ShootBullet(data.bulletPrefab, firePoint.position, dir, BulletOwner.Enemy);
    }

    void ResetRandomTimer()
    {
        nextShootTimer = Random.Range(data.minShootDelay, data.maxShootDelay);
    }

}
