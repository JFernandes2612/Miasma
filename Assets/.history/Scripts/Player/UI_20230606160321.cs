using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField]
    private bool isMenuOpen;

    // Start is called before the first frame update
    void Start()
    {
        isMenuOpen = false;
        Debug.Log(isMenuOpen);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            isMenuOpen = true;
        }
        else {
            isMenuOpen = false;
        }
        Debug.Log(isMenuOpen);
    }
}
