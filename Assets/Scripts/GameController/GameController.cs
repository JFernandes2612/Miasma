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

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load next scene
    }
}
