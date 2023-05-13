using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    void Awake() {
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

    public void NextLevel()
    {
        StartCoroutine(LoadNextSceneAsync());
    }
}
