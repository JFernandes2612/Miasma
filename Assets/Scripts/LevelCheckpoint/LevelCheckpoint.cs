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
        if (checkEnemyDependency()) // SHOW PLAYER CANNOT CONTINUE IF NOT ALL ENEMY ARE KILLED
            return;

        if (other.gameObject.tag == "Player")
        {
            unityEvent.Invoke();
            gameObject.SetActive(false);
        }
    }
}
