using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenMessage : MonoBehaviour
{
    private FloatingMessage floatingMessage;

    [SerializeField]
    private float delay;

    [SerializeField]
    private bool pulse;

    [SerializeField]
    private string text;

    private bool triggered = false;

    private void Start() {
        floatingMessage = GameObject.Find("OnScreenMessage").GetComponent<FloatingMessage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !triggered)
        {
            StartCoroutine(RunEvents());
        }
    }

    IEnumerator RunEvents()
    {
        triggered = true;
        floatingMessage.SetPulse(pulse);
        floatingMessage.SetText(text);
        floatingMessage.SetActive(true);
        floatingMessage.FadeIn();
        yield return new WaitForSeconds(delay);
        floatingMessage.FadeOut();
        gameObject.SetActive(false);
    }
}
