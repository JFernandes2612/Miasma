using UnityEngine;

public class StickCollisionDetection : MonoBehaviour
{
    public float damage;
    private void OnCollisionEnter(Collision collision)
    {
        //Play Enemy Shot Colision Sound


        if (collision.gameObject.CompareTag("Player"))
        {

            // Get the Player component from the collided object
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && transform.root.gameObject.GetComponent<Enemy>()!= null)
            {
                // Call the TakeDamage function on the Player object
                player.TakeDamage(damage + transform.root.gameObject.GetComponent<Enemy>().extraDamage);
            }
        }

        // Check if the collided object has a tag to ignore collisions with
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Ignore the collision with the tagged object
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
