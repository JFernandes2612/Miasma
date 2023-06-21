using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapierBehaviour : MonoBehaviour
{


    [SerializeField]
    private float swordDamage = 1.0f;

    public GameObject hitEffect;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            return;
        }

        if (other.transform.TryGetComponent<Entity>(out Entity T))
        {
            var collisionPoint = other.ClosestPoint(transform.position);
            GameObject GO = Instantiate(hitEffect, collisionPoint, Quaternion.identity);
            GO.transform.parent = T.gameObject.transform;
            Destroy(GO, 20);
            T.TakeDamage(swordDamage);
        }
    }
}
