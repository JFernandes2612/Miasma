using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    private GameObject player;

    private bool loadingScene = false;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject.FindGameObjectWithTag("Player").transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    void Update() {
        // If player falls down the level
        if (player.transform.position.y <= -10 && !loadingScene) {
            player.GetComponent<Player>().TakeDamage(9999);
        }
    }

    IEnumerator LoadNextSceneAsync()
    {
        loadingScene = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadingScene = false;
    }

    IEnumerator ReloadCurrentSceneAsync()
    {
        loadingScene = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadingScene = false;
    }

    public void NextLevel()
    {
        StartCoroutine(LoadNextSceneAsync());
    }

    public void ReloadLevel()
    {
        GameObject.Find("WeaponHolder").GetComponent<WeaponSwitching>().ResetWeapons();
        StartCoroutine(ReloadCurrentSceneAsync());
    }
}
