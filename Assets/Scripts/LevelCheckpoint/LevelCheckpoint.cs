using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelCheckpoint : MonoBehaviour
{
    [SerializeField]
    private UnityEvent unityEvent;
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player")
            unityEvent.Invoke();

        gameObject.SetActive(false);
    }
}
