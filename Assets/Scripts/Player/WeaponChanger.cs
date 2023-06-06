using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{

    [SerializeField]
    private GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            menu.SetActive(true);
            Debug.Log(Input.mousePosition);
        }
        if (Input.GetKeyUp(KeyCode.F)){
            Cursor.visible = false;
            menu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
