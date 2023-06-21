using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerProjectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float damage = 1f;

    [SerializeField]
    private float lifeTime = 5f;

    private Rigidbody rb;

    private Vector3 direction;

    public GameObject hitEffect;

    internal void SetDamage(float m1AttackDamage)
    {
        damage = m1AttackDamage;
    }

    internal void SetDirection(Vector3 forward)
    {
        direction = forward;
    }

    internal void SetSpeed(float v)
    {
        speed = v;
    }

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);

    }


    // Update is called once per frame
    void Update()
    {
        rb.AddForce(direction * speed, ForceMode.Impulse);
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
            Destroy(gameObject);
            GO.transform.parent = T.gameObject.transform;
            Destroy(GO, 10);
            T.TakeDamage(damage);

        }



    }
}
