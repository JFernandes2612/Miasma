using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Entity : MonoBehaviour
{
    private float currentHealth;

    [SerializeField]
    protected float maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth > 0.0f)
        {
            currentHealth -= amount;

            if (currentHealth <= 0.0f)
            {
                Death();
            }
        }
    }

    abstract protected void Death();
}
