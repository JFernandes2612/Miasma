using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{

    private void Start() {
        InputSystem.DisableDevice(Keyboard.current);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu() {
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
