using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyDependency;

    private FloatingMessage floatingMessage;

    private void Start()
    {
        floatingMessage = GameObject.Find("OnScreenMessage").GetComponent<FloatingMessage>();
    }

    private bool checkEnemyDependency()
    {
        foreach (GameObject item in enemyDependency)
        {
            foreach (Transform maybeEnemy in item.transform)
                if (maybeEnemy.gameObject.CompareTag("Enemy"))
                    return true;
        }

        return false;
    }

    private void OnCollisionEnter(Collision other)
    {

        if (checkEnemyDependency())
        {
            StartCoroutine(KillThemAll());
            return;
        }

        if (other.gameObject.tag == "Player")
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().NextLevel();
    }

    private IEnumerator KillThemAll()
    {
        floatingMessage.SetPulse(true);
        floatingMessage.SetText("KILL THEM ALL\nKEEP NO ONE ALIVE");
        floatingMessage.SetActive(true);
        floatingMessage.FadeIn();
        yield return new WaitForSeconds(5);
        floatingMessage.FadeOut();
    }
}
