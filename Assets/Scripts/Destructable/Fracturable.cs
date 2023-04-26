using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracturable : Entity
{
    private Fracture fracture;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        fracture = GetComponent<Fracture>();
        rb = GetComponent<Rigidbody>();
    }

    protected override void Death()
    {
        fracture.CauseFracture();
    }

    public void explodeFracturePieces()
    {
        float explosionRadius = Mathf.Max(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y), transform.lossyScale.z);
        Collider[] objects = UnityEngine.Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider h in objects)
        {
            Rigidbody r = h.GetComponent<Rigidbody>();
            if (r != null)
            {
                r.AddExplosionForce(rb.mass * 10.0f, transform.position, explosionRadius);
            }
        }
    }
}
