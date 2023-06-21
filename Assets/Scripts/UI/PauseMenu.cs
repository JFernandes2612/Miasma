using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject continueBtt;

       [SerializeField]
    private GameObject quitBtt;

    // Start is called before the first frame update
    void Start()
    {
        Button button = continueBtt.GetComponent<Button>();
        button.onClick.AddListener(Unpause);

        Button button1 = quitBtt.GetComponent<Button>();
        button1.onClick.AddListener(Quit);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P) && !pauseMenu.activeSelf){
            pauseMenu.SetActive(true);
            InputSystem.DisableDevice(Keyboard.current);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Other pause logic...

        }
    }


    void Unpause(){
        pauseMenu.SetActive(false);
        InputSystem.EnableDevice(Keyboard.current);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Other unpause logic...
    }


    void Quit(){
        // Go to main menu
    }
}
