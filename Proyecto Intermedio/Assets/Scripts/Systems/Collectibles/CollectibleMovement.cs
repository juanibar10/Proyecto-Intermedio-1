using UnityEngine;

//Clase creada únicamente para la prueba de Score System
//TODO-Se eliminará a futuro
public class CollectibleMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
