using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSurface : MonoBehaviour
{
    protected float damage;

    private void DamageEntity(GameObject gameObject)
    {
       
        if (gameObject.tag == "Player")
        {
            gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision other)
    {
       
        DamageEntity(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        DamageEntity(other.gameObject);
    }
}
