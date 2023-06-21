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
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(KeyCode.F))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isMenuOpen = true;
            Debug.Log(mouseX + " " + mouseY);
        }
        if (Input.GetKeyUp(KeyCode.F)){
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isMenuOpen = false;
        }
    }
}
