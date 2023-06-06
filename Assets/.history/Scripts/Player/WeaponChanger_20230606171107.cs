using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{

    [SerializeField]
    private bool isMenuOpen;

    public Look look;

    // Start is called before the first frame update
    void Start()
    {
        isMenuOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            look.enabled = false;
            isMenuOpen = true;
            Debug.Log(isMenuOpen);
        }
        if (Input.GetKeyUp(KeyCode.F)){
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            look.enabled = true;
            isMenuOpen = false;
            Debug.Log(isMenuOpen);
        }
    }
}
