using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracturable : Entity
{
    private Fracture fracture;

    private Rigidbody rb;

    [SerializeField]
    private float fadeOutTime = 1f;

    [SerializeField]
    private int fadeOutTimeSteps = 100;

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

    public void ExplodeFracturePieces()
    {
        float explosionRadius = Mathf.Max(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y), transform.lossyScale.z);
        Collider[] objects = UnityEngine.Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider h in objects)
        {
            Rigidbody r = h.GetComponent<Rigidbody>();
            if (r != null)
            {
                r.AddExplosionForce(rb.mass * 15.0f * explosionRadius, transform.position, explosionRadius);
            }
        }
    }

    public void ChangeFragmentsLayer() {
        gameObject.SetActive(true);
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        for (int child = 0; child < transform.parent.GetChild(1).childCount; child++) {
            transform.parent.GetChild(1).GetChild(child).gameObject.layer = LayerMask.NameToLayer("Fragment");
            foreach (Material material in transform.parent.GetChild(1).GetChild(child).gameObject.GetComponent<Renderer>().materials)
            {
                StartCoroutine(FadeOut(material));
            }
        }
    }

    IEnumerator FadeOut(Material material) {
        float time = 0;
        float step = fadeOutTime / fadeOutTimeSteps;

        while (time <= fadeOutTime + step)
        {
            material.color = new Color(material.color.r, material.color.g, material.color.b, 1.0f - time/fadeOutTime);
            time += step;
            yield return new WaitForSecondsRealtime(step);
        }

        material.color = new Color(material.color.r, material.color.g, material.color.b, 0);
    }
}
