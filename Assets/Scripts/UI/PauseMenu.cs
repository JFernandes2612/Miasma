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

    private WeaponSwitching weaponSwitching;

    private GameObject enemies;

    private Movement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        weaponSwitching = GameObject.Find("WeaponHolder").GetComponent<WeaponSwitching>();
        enemies = GameObject.Find("Enemies");
        mainCamLook = Camera.main.GetComponent<Look>();
        noPosEffectLook = GameObject.Find("WeaponCameraNoPosEffects").GetComponent<Look>();
        Button button = continueBtt.GetComponent<Button>();
        button.onClick.AddListener(Unpause);

        playerMovement = GameObject.Find("Player").GetComponent<Movement>();

        Button button1 = quitBtt.GetComponent<Button>();
        button1.onClick.AddListener(Quit);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            if (!pauseMenu.activeSelf)
            {
                Pause();
            }


        }
    }

    void pausePlayerMovement()
    {
        playerMovement.isAnimLocked = true;

    }

    void unPausePlayerMovement()
    {
        playerMovement.isAnimLocked = false;
    }

    void pauseAllEnemiesMovement()
    {
        if (enemies == null) return;
        foreach (Transform child in enemies.transform)
        {
            child.GetComponent<Enemy>().Freeze();
        }
    }

    void unPauseAllEnemiesMovement()
    {
        if (enemies == null) return;
        foreach (Transform child in enemies.transform)
        {
            child.GetComponent<Enemy>().UnFreeze();
        }
    }

    void Pause()
    {
        Debug.Log("Pause");
        mainCamLook.LockCamera();
        noPosEffectLook.LockCamera();
        pauseMenu.SetActive(true);
        weaponSwitching.disableCurrentWeapon();
        pauseAllEnemiesMovement();
        pausePlayerMovement();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Other pause logic...
    }


    void Unpause()
    {
        Debug.Log("UnPause");
        mainCamLook.UnlockCamera();
        noPosEffectLook.UnlockCamera();
        pauseMenu.SetActive(false);
        weaponSwitching.enableCurrentWeapon();
        unPauseAllEnemiesMovement();
        unPausePlayerMovement();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Other unpause logic...
    }


    void Quit()
    {
        Debug.Log("Quit the game");
        //disable the event system
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
