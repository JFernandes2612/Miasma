using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Tiling : MonoBehaviour
{
    [SerializeField]
    private float tilingFactor = 2.5f;

    void Start()
    {
        GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(transform.lossyScale.x / tilingFactor, transform.lossyScale.z / tilingFactor);
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.hasChanged && Application.isEditor && !Application.isPlaying)
        {
            GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(transform.lossyScale.x / tilingFactor, transform.lossyScale.z / tilingFactor);
            transform.hasChanged = false;
        }

    }
}
