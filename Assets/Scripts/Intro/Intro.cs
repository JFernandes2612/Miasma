using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Intro : MonoBehaviour
{
     [SerializeField]
    private GameObject waitScreen;
    void OnEnable()
    {
        waitScreen.SetActive(true);
        StartCoroutine(StartLevel1Async());
    }


    IEnumerator StartLevel1Async()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
