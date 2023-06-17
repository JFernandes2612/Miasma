using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    private GameObject player;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject.FindGameObjectWithTag("Player").transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    IEnumerator LoadNextSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator ReloadCurrentSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
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
