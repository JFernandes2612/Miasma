using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Tiling : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(transform.lossyScale.x / 2.5f, transform.lossyScale.z / 2.5f);
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.hasChanged && Application.isEditor && !Application.isPlaying)
        {
            GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(transform.lossyScale.x / 2.5f, transform.lossyScale.z / 2.5f);
            transform.hasChanged = false;
        }

    }
}
