using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class FistAttack : Weapon
{
    [SerializeField]
    private float attackRange = 3f;

    [SerializeField]
    private float attackDelay = 0.5f;

    [SerializeField]
    private float attackDamage = 1f;

    [SerializeField]
    private float attackCooldown = 1f; // attack Speed

    // create fmod references
    public FMODUnity.EventReference fistSwingEvent;
    public FMODUnity.EventReference fistHitEvent;

    private float attackTimer;
    string currentAnimationState;

    public const string IDLE = "IdlePunch";
    public const string RIGHT_PUNCH = "RightPunch";

    void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        playerAttack = new PlayerInput();
        playerAttack.Enable();
    }

    public override  float getLMBCooldown(){
        return attackDelay*2;
    }

    public override  float getRMBCooldown(){
        return 0;
    }

    public override  bool isLMBCooldown(){
    return isAttacking;
    }

    public override  bool isRMBCooldown(){
        return false;
    }



    public void ChangeAnimationState(string newState)
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        //if (currentAnimationState == newState) return;

        // PLAY THE ANIMATION //
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }

    void OnEnable()
    {

        playerAttack.Player_Map.Attack.performed += Attack_M1;

    }

    void OnDisable()
    {

        playerAttack.Player_Map.Attack.performed -= Attack_M1;

    }

    private void Update()
    {

        SetAnimations();
    }

    void ResetAttack()
    {
        isAttacking = false;

    }
    void SetAnimations()
    {
        // If player is not attacking
        if (!isAttacking)
        {
            ChangeAnimationState(IDLE);
        }
    }

    public override void Attack_M1(CallbackContext ctx)
    {
        if (isAttacking) return;

        isAttacking = true;

        Invoke(nameof(ResetAttack), attackCooldown);
        StartCoroutine(AttackRaycast(attackRange, attackDamage, attackDelay));
        // play fist swing event
        FMODUnity.RuntimeManager.PlayOneShot(fistSwingEvent);
        ChangeAnimationState(RIGHT_PUNCH);
    }

    public override void Attack_M2(CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}
