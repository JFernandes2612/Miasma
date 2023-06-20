using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBox : Fracturable
{
    protected override void Death()
    {
        fracture.CauseFracture();
        GameObject.Find("Player").GetComponent<Player>().AddRedPoints(1);
    }
}
