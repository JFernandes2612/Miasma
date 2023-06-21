using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    [ReadOnlyAttribute]
    [SerializeField]
    private bool invincible = false;

    private int redPoints = 0;

    private int yellowPoints = 0;

    private Movement movementScript;

    public FMODUnity.EventReference deathEffectsEvent;
    private FMOD.Studio.EventInstance deathEffectsInstance;

    public void AddRedPoints(int points)
    {
        redPoints += points;
    }

    public void AddYellowPoints(int points)
    {
        yellowPoints += points;
    }

    public void SetRedPoints(int points) {
        redPoints = points;
    }

    public void SetYellowPoints(int points) {
        yellowPoints = points;
    }

    public int GetRedPoints()
    {
        return redPoints;
    }

    public int GetYellowPoints()
    {
        return yellowPoints;
    }

    new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(transform.gameObject);

        movementScript = GetComponent<Movement>(); // for changing movement sound effects on death
    }

    public void setInvincible(bool value)
    {
        invincible = value;
    }

    public new void TakeDamage(float damage)
    {
        if (!invincible)
            base.TakeDamage(damage);
    }

    protected override void Death()
    {
        movementScript.SetFmodSpeed(0.0f);
        deathEffectsInstance = FMODUnity.RuntimeManager.CreateInstance(deathEffectsEvent);
        deathEffectsInstance.start();
        movementScript.SetFmodSpeed(0.0f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ReloadLevel();
        if (SceneManager.GetActiveScene().buildIndex == 1)
            Destroy(gameObject);
        Awake();
    }
}
