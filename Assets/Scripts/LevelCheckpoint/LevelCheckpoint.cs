using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelCheckpoint : MonoBehaviour
{
    [SerializeField]
    private UnityEvent unityEvent;
    // Start is called before the first frame update

    [SerializeField]
    private GameObject[] enemyDependency;

    private FloatingMessage floatingMessage;

    public FMODUnity.EventReference checkpointEvent; // wind + lightsaberish effects
    private FMOD.Studio.EventInstance checkpointInstance;

    private void Start() {
        checkpointInstance = FMODUnity.RuntimeManager.CreateInstance(checkpointEvent);
        floatingMessage = GameObject.Find("OnScreenMessage").GetComponent<FloatingMessage>();
    }

    private bool checkEnemyDependency() {
        foreach (GameObject item in enemyDependency)
        {
            foreach (Transform maybeEnemy in item.transform)
            if (maybeEnemy.gameObject.CompareTag("Enemy"))
                return true;
        }

        return false;
    }

    private void OnCollisionEnter(Collision other) {
        if (checkEnemyDependency()) {
            StartCoroutine(KillThemAll());
            return;
        }

        if (other.gameObject.tag == "Player")
        {
            unityEvent.Invoke();
            checkpointInstance.start();
            gameObject.SetActive(false);
        }
    }

    private IEnumerator KillThemAll() {
        floatingMessage.SetPulse(true);
        floatingMessage.SetText("KILL THEM ALL\nKEEP NO ONE ALIVE");
        floatingMessage.SetActive(true);
        floatingMessage.FadeIn();
        yield return new WaitForSeconds(5);
        floatingMessage.FadeOut();
    }
}
