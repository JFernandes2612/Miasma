using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour
{

    // Start is called before the first frame update

    [SerializeField]
    private float swordDamage = 1.0f;

    public GameObject hitEffect;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

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
