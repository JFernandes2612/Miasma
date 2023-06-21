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

    private FMODHelper fmodHelper = new FMODHelper();
    public FMODUnity.EventReference moveEffectsEvent; // wind + lightsaberish effects
    private FMOD.Studio.EventInstance moveEffectsInstance;
    FMOD.Studio.PARAMETER_ID speedID;

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

        // create fmod instances, and get speed parameter ID
        moveEffectsInstance = FMODUnity.RuntimeManager.CreateInstance(moveEffectsEvent);
        moveEffectsInstance.start();
        speedID = fmodHelper.GetParameterID(moveEffectsInstance, "speed");
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
        moveEffectsInstance.setParameterByID(speedID, 0.0f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ReloadLevel();
        if (SceneManager.GetActiveScene().buildIndex == 0)
            Destroy(gameObject);
        Awake();
    }
}
