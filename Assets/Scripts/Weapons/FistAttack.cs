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
    private float attackCooldown = 0.6f; // attack Speed

    private float attackCooldownCounter = 0f;

    private float M1AnimationDuration = 1f;

    private ColliderWeaponsBehavior colliderWeaponsBehavior;

    private BoxCollider fistCollider;

    private float attackTimer;
    string currentAnimationState;

    public const string IDLE = "IdlePunch";
    public const string RIGHT_PUNCH = "RightPunch";

    override protected void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        cam = Camera.main;
        playerAttack = new PlayerInput();
        playerAttack.Enable();
        fistCollider = GetComponentInChildren<BoxCollider>();

        colliderWeaponsBehavior = GetComponentInChildren<ColliderWeaponsBehavior>();
        colliderWeaponsBehavior.colliderDamage = attackDamage;
    }

    public override float getLMBCooldown()
    {
        return attackCooldownCounter / M1AnimationDuration;
    }

    public override float getRMBCooldown()
    {
        return 0;
    }

    public override bool isLMBCooldown()
    {
        return isAttacking;
    }

    public override bool isRMBCooldown()
    {
        return false;
    }


    private void HandleM1CoolDown()
    {
        if (attackCooldownCounter > 0)
        {
            attackCooldownCounter -= Time.deltaTime;
        }

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

    public void enableFistCollider()
    {
        fistCollider.enabled = true;
    }

    public void disableFistCollider()
    {
        fistCollider.enabled = false;
    }

    private void Update()
    {

        SetAnimations();
        HandleM1CoolDown();
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
        attackCooldownCounter = M1AnimationDuration;

        Invoke(nameof(ResetAttack), attackCooldown);

        ChangeAnimationState(RIGHT_PUNCH);
    }

    public override void Attack_M2(CallbackContext context)
    {
        Debug.Log("FistAttack: Attack_M2 not implemented");
    }

    public override void enableScript()
    {
        OnEnable();
    }

    public override void disableScript()
    {
        OnDisable();
    }
}
