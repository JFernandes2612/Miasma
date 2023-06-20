using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    public GameObject impactEffect;
    private void OnCollisionEnter(Collision collision)
    {
        //Play Enemy Shot Colision Sound


        //FindObjectOfType<AudiManager>().Play("Explosion");
        //GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        //Destroy(impact, 2);
        // Destroy the object when it collides with anything
        Destroy(gameObject);
    }
}
