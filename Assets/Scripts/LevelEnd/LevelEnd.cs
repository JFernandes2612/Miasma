using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().NextLevel();
    }
}
