using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class BroadswordAttack : Weapon
{
    // For both attacks
    [SerializeField]
    private float attackCooldown = 3f;

    [SerializeField]
    private float swordDamage = 5f;

    private float animationsDuration = 1.8f;
    private float attackCooldownCounter = 0f;

    // First attack
    [SerializeField]
    private float smallSwingAttackDelay = 0.3f; // Time between the input action and applying the lunge force
    [SerializeField]
    private float smallSwingLungeTime = 0.5f; // Time the lunge force is applied for
    [SerializeField]
    private float smallSwingLungeForce = 2f; // Lunge force

    // Second attack
    [SerializeField]
    private float upperSwingAttackDelay = 0.5f; // Time between the input action and applying the lunge force
    [SerializeField]
    private float upperSwingLungeTime = 2f; // Time the lunge force is applied for
    [SerializeField]
    private float upperSwingLungeForce = 3f; // Lunge force

    [SerializeField]
    private float defendDuration = 2.0f; // Invincibility duration (animation is reset at the end)

    private float defendDurationCounter = 0f;

    private int CountAttack;             // 0 or 1 (first attack or second attack)

    private Movement playerMovement;
    private Player playerScript;

    [SerializeField]
    private Rigidbody playerRb;

    private BoxCollider swordCollider;

    private const string SWING = "Broadsword Swing";

    private const string UPPER_SWING = "Broadsword Jump Swing";

    private Camera noWeaponEffectsCam;

    private ColliderWeaponsBehavior colliderWeaponsBehavior;

    override protected void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        cam = Camera.main;
        playerAttack = new PlayerInput();
        playerAttack.Enable();
        CountAttack = 0;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<Movement>();
        playerScript = player.GetComponent<Player>();
        noWeaponEffectsCam = GameObject.Find("WeaponCameraNoPosEffects").GetComponent<Camera>();
        swordCollider = gameObject.GetComponentInChildren<BoxCollider>();

        playerRb = player.GetComponent<Rigidbody>();
        colliderWeaponsBehavior = GetComponentInChildren<ColliderWeaponsBehavior>();
        colliderWeaponsBehavior.colliderDamage = swordDamage;
    }

    void OnEnable()
    {

        playerAttack.Player_Map.Attack.performed += Attack_M1;
        playerAttack.Player_Map.SpecialAttack.performed += Attack_M2;
    }

    void OnDisable()
    {

        swordCollider.enabled = false;
        playerAttack.Player_Map.Attack.performed -= Attack_M1;
        playerAttack.Player_Map.SpecialAttack.performed -= Attack_M2;
    }

    void HandleM1CoolDown()
    {
        if (attackCooldownCounter > 0)
        {
            attackCooldownCounter -= Time.deltaTime;
        }
    }

    void HandleM2CoolDown()
    {
        if (defendDurationCounter > 0)
        {
            defendDurationCounter -= Time.deltaTime;
        }
    }

    void Update()
    {
        if (CountAttack == 1)
        {
            animator.SetInteger("attackPhase", 1);

            //PLAY THE SWING SOUND HERE
            isAttacking = true;

            Vector3 lungeDirection = new Vector3(transform.forward.x, 0, transform.forward.z);
            //StartCoroutine(ApplyLunge(smallSwingAttackDelay, smallSwingLungeTime, smallSwingLungeForce, lungeDirection));
        }
        HandleM1CoolDown();
        HandleM2CoolDown();
    }

    public void UpdateCountDownTimer()
    {

        isAttacking = true;
        attackCooldownCounter = animationsDuration;
    }

    public override void Attack_M1(CallbackContext context)
    {
        if (isAttacking && CountAttack == 0) return;
        if (CountAttack < 2) CountAttack++;

    }

    public void CheckAttackPhase()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(SWING))
        {
            if (CountAttack > 1)
            {
                animator.SetInteger("attackPhase", 2);
                //play the upper swing SOUND
                Vector3 lungeDirection = Vector3.up;
                //StartCoroutine(ApplyLunge(upperSwingAttackDelay, upperSwingLungeTime, upperSwingLungeForce, lungeDirection));
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName(UPPER_SWING))
        {
            if (CountAttack >= 2)
            {
                ResetAttackPhase();
            }

        }

    }

    public override void Attack_M2(CallbackContext context)
    {
        if (CountAttack != 0 || !readyToM2) return;
        readyToM2 = false;

        defendDurationCounter = defendDuration;
        animator.SetInteger("attackPhase", 3);
        playerScript.setInvincible(true);

        StartCoroutine(ResetDefendLockIn(defendDuration));
    }


    public void PlayerAnimLock()
    {
        playerMovement.isAnimLocked = true;
    }

    public void PlayerAnimUnlock()
    {
        playerMovement.isAnimLocked = false;
    }

    // Applies a force with direction direction for lungetime seconds, after a delay of attackDelay seconds
    private IEnumerator ApplyLunge(float attackDelay, float lungeTime, float lungeForce, Vector3 lungeDirection)
    {
        //apply force forwards

        yield return new WaitForSeconds(attackDelay);
        playerRb.velocity = lungeDirection * lungeForce;
        yield return new WaitForSeconds(lungeTime);
        playerRb.velocity = Vector3.zero;

    }

    public void enableSwordCollider()
    {
        swordCollider.enabled = true;
    }

    public void disableSwordCollider()
    {
        swordCollider.enabled = false;
    }

    private IEnumerator ResetDefendLockIn(float defendDuration)
    {
        yield return new WaitForSeconds(defendDuration);
        playerScript.setInvincible(false);
        readyToM2 = true;
        animator.SetInteger("attackPhase", 0);
    }

    private void ResetAttackPhase()
    {
        animator.SetInteger("attackPhase", 0);
        disableSwordCollider();
        CountAttack = 0;
        attackCooldownCounter = attackCooldown;
        StartCoroutine(ResetAttackLockIn(attackCooldown));
    }
    public int getAttackPhase()
    {
        return CountAttack;
    }
    public override float getLMBCooldown()
    {
        return attackCooldownCounter / attackCooldown;
    }

    public override bool isLMBCooldown()
    {
        return attackCooldownCounter > 0;
    }

    public override float getRMBCooldown()
    {
        return defendDurationCounter / defendDuration;
    }

    public override bool isRMBCooldown()
    {
        return defendDurationCounter > 0;
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
