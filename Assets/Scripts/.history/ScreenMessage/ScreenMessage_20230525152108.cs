using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMessage : MonoBehaviour
{

    private UnityEvent UnityEvent;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CanvasObject.SetActive(true);
        }
    }
}
