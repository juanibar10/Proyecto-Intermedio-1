using UnityEngine;

public static class BulletHelper
{
    public static Bullet ShootBullet(GameObject prefab, Vector3 position, int direction, BulletOwner owner, float speedOverride = -1)
    {
        GameObject bullet = Object.Instantiate(prefab, position, Quaternion.identity);

        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null)
        {
            b.direction = direction;
            b.owner = owner;

            if (speedOverride > 0)
            {
                b.speed = speedOverride;
            }  
        }
        return b;
    }
}
