using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSurface : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Player>().TakeDamage(9999.9f);
        }
    }
}
