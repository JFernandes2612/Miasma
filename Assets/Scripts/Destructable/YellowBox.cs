using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBox : Fracturable
{
    protected override void Death()
    {
        fracture.CauseFracture();
        GameObject.Find("Player").GetComponent<Player>().AddYellowPoints(1);
    }
}
