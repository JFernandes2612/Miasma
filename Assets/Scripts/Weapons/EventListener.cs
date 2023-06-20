using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{

    private RapierAttack rapierAttack;

    void Start()
    {
        rapierAttack = GetComponent<RapierAttack>();
    }

    public void AnimEvent()
    {
        rapierAttack.CheckAttackPhase();
    }
    public void AttackHitbox()
    {
        rapierAttack.M1AttackHitBox();
    }
}
