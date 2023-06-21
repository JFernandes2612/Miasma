using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{

    [SerializeField]
    private bool isMenuOpen;

    // Start is called before the first frame update
    void Start()
    {
        isMenuOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            isMenuOpen = true;
            Debug.Log(isMenuOpen);
        }
        else if (Input.GetKey(KeyCode.F)){
            isMenuOpen = false;
            Debug.Log(isMenuOpen);
        }
    }
}
