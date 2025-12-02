using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] public float health = 10;

    public void takeDamage(float damage, GameObject sourceDamageObject)
    {
        health -= damage;
        if (health <= 0)
        {
            //SpawnPool.Instance.Despawn(GameObject);
        }
    }


    private void onTriggerEnter(Collider other)
    {

    }
}