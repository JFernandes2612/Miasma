using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracturable : Entity
{
    protected Fracture fracture;

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

    private void ExplodeFracturePieces()
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

    public void FragmentFadeOut() {
        gameObject.SetActive(true);
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        for (int child = 0; child < transform.parent.GetChild(1).childCount; child++) {
            transform.parent.GetChild(1).GetChild(child).gameObject.layer = LayerMask.NameToLayer("Fragment");
            StartCoroutine(FadeOut(transform.parent.GetChild(1).GetChild(child).gameObject));
        }

        ExplodeFracturePieces();
    }

    private IEnumerator FadeOut(GameObject gameObject) {
        Material[] materials = gameObject.GetComponent<Renderer>().materials;

        float time = 0;
        float step = fadeOutTime / fadeOutTimeSteps;

        while (time <= fadeOutTime + step)
        {
            foreach (Material material in materials)
                material.color = new Color(material.color.r, material.color.g, material.color.b, 1.0f - time/fadeOutTime);
            time += step;
            yield return new WaitForSeconds(step);
        }

        foreach (Material material in materials)
            material.color = new Color(material.color.r, material.color.g, material.color.b, 0);

        Destroy(gameObject, 0.1f);
    }
}
