using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenMessage : MonoBehaviour
{
    [SerializeField]
    private UnityEvent startEvents;

    [SerializeField]
    private UnityEvent endEvents;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(RunEvents());
        }
    }

    IEnumerator RunEvents() 
    {
        startEvents.Invoke();
        yield return new WaitForSeconds(1);
        endEvents.Invoke();
    }
}
