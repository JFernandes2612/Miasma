using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the object when it collides with anything
        Destroy(gameObject);
    }
}
