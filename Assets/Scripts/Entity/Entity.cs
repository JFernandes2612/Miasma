using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

abstract public class Entity : MonoBehaviour
{
    [SerializeField]
    public float currentHealth;

    [SerializeField]
    protected float maxHealth;

    protected void Awake()
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
