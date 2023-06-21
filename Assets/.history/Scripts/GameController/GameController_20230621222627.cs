using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    private GameObject player;

    private int saveYellowPoints = 0;
    private int saveRedPoints = 0;

    public FMODUnity.EventReference backgroundEvent;
    private FMOD.Studio.EventInstance backgroundInstance;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject.FindGameObjectWithTag("Player").transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    private void Update() {
        if (SceneManager.GetActiveScene().buildIndex == 7) {
            if (GameObject.FindWithTag("Enemy") == null)
                GameObject.FindWithTag("GameEnd").SetActive(true);
        }
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
        saveRedPoints = GameObject.Find("Player").GetComponent<Player>().GetRedPoints();
        saveYellowPoints = GameObject.Find("Player").GetComponent<Player>().GetYellowPoints();
        GameObject.Find("OnScreenMessage").GetComponent<FloatingMessage>().Reset();
        StartCoroutine(LoadNextSceneAsync());
    }

    public void ReloadLevel()
    {
        GameObject.Find("Player").GetComponent<Player>().SetYellowPoints(saveYellowPoints);
        GameObject.Find("Player").GetComponent<Player>().SetRedPoints(saveRedPoints);
        GameObject.Find("WeaponHolder").GetComponent<WeaponSwitching>().ResetWeapons();
        GameObject.Find("OnScreenMessage").GetComponent<FloatingMessage>().Reset();
        StartCoroutine(ReloadCurrentSceneAsync());
    }
}
