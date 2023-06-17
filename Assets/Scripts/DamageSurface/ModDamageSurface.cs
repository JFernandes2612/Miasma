using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModDamageSurface : DamageSurface
{
    [SerializeField]
    private float surfaceDamage;

    private void Start() {
        damage = surfaceDamage;
    }
}
