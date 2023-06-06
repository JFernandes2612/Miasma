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
        if (Input.GetButtonDown(KeyCode.F))
        {
            isMenuOpen = true;
            Debug.Log(isMenuOpen);
        }
        else if (Input.GetButtonUp(KeyCode.F)){
            isMenuOpen = false;
            Debug.Log(isMenuOpen);
        }
    }
}
