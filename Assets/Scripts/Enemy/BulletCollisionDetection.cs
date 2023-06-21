using UnityEngine;

public class BulletCollisionDetection : MonoBehaviour
{
    public float damage;
    public GameObject impactEffect;
    private void OnCollisionEnter(Collision collision)
    {
        //Play Enemy Shot Colision Sound


        if (collision.gameObject.CompareTag("Player"))
        {

            // Get the Player component from the collided object
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                // Call the TakeDamage function on the Player object
                player.TakeDamage(damage);
            }
        }


        //GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        //Destroy(impact, 2);
        // Destroy the object when it collides with anything
        Destroy(gameObject);
    }
}
