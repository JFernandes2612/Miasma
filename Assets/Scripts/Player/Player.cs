using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    [ReadOnlyAttribute]
    [SerializeField]
    private bool invincible = false;

    new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(transform.gameObject);
    }

    public void setInvincible(bool value) {
        invincible = value;
    }

    public new void TakeDamage(float damage) {
        if (!invincible)
            base.TakeDamage(damage);
    }

    protected override void Death()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ReloadLevel();
        if (SceneManager.GetActiveScene().buildIndex == 0)
            Destroy(gameObject);
        Awake();
    }
}
