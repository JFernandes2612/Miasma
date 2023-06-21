using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    private void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Destroy(GameObject.Find("Player"));
    }

    public void StartGame() {
        InputSystem.EnableDevice(Keyboard.current);
        StartCoroutine(StartGameAsync());
    }

    public void QuitGame() {
        Application.Quit();
    }

    IEnumerator StartGameAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
