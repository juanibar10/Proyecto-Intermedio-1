using UnityEngine;

public static class BulletHelper
{
    public static void ShootBullet(GameObject bulletPrefab, Vector3 position, int dir, float speedOverride = -1)
    {
        GameObject bullet = Object.Instantiate(bulletPrefab, position, Quaternion.identity);
        Bullet b = bullet.GetComponent<Bullet>();

        if (b != null)
        {
            b.direction = dir;
            if (speedOverride > 0)
                b.speed = speedOverride;
        }
    }
}
