using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public abstract class Weapon : MonoBehaviour
{

    protected Animator animator;

    protected bool isAttacking = false;

    protected PlayerInput playerAttack;
    protected bool readyToM2 = true;
    public Camera cam;

    public abstract float getLMBCooldown();

    public abstract bool isLMBCooldown();

    public abstract float getRMBCooldown();

    public abstract bool isRMBCooldown();

    public abstract void enableScript();

    public abstract void disableScript();

    public bool getIsAttacking(){
        return isAttacking;
    }
    public FMODUnity.EventReference swingEffectsEvent;
    private FMOD.Studio.EventInstance swingEffectsInstance;

    public FMODUnity.EventReference fistEffectsEvent;
    private FMOD.Studio.EventInstance fistEffectsInstance;

    protected virtual void Awake()
    {

        swingEffectsInstance = FMODUnity.RuntimeManager.CreateInstance(swingEffectsEvent);
        fistEffectsInstance = FMODUnity.RuntimeManager.CreateInstance(fistEffectsEvent);
    }



    protected IEnumerator ResetAttackLockIn(float attackCooldown)
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }


    abstract public void Attack_M1(CallbackContext context);
    abstract public void Attack_M2(CallbackContext context);

    public void PlayFistAttackSFX()
    {
        fistEffectsInstance.start();
    }

    public void PlayWeaponAttackSFX()
    {
        swingEffectsInstance.start();
    }

}
