using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player")
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().NextLevel();
    }
}
