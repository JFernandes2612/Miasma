using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject continueBtt;

    [SerializeField]
    private GameObject quitBtt;

    private Look mainCamLook;

    private Look noPosEffectLook;

    // Start is called before the first frame update
    void Start()
    {
        mainCamLook = Camera.main.GetComponent<Look>();
        noPosEffectLook = GameObject.Find("WeaponCameraNoPosEffects").GetComponent<Look>();
        Button button = continueBtt.GetComponent<Button>();
        button.onClick.AddListener(Unpause);

        Button button1 = quitBtt.GetComponent<Button>();
        button1.onClick.AddListener(Quit);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P) && !pauseMenu.activeSelf)
        {
            mainCamLook.LockCamera();
            noPosEffectLook.LockCamera();
            pauseMenu.SetActive(true);
            InputSystem.DisableDevice(Keyboard.current);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Other pause logic...

        }
    }


    void Unpause()
    {
        mainCamLook.UnlockCamera();
        noPosEffectLook.UnlockCamera();
        pauseMenu.SetActive(false);
        InputSystem.EnableDevice(Keyboard.current);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Other unpause logic...
    }


    void Quit()
    {
        StartCoroutine(MainMenuAsync());
    }

    IEnumerator MainMenuAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
