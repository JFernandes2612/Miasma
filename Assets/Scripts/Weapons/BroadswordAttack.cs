using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class BroadswordAttack : Weapon
{
    [SerializeField]
    private float smallAttackCooldown = 2.3f;
    [SerializeField]
    private float smallSwingAttackDelay = 1.1f;
    [SerializeField]
    private float smallSwingLungeTime = 0.5f;
    [SerializeField]
    private float smallSwingLungeForce = 20f;
    [SerializeField]
    private float smallSwingAttackDamage = 5.0f;

    [SerializeField]
    private float smallSwingAttackRange = 2f;

    [SerializeField]
    private float defendCooldown = 2.0f;

    //public FMODUnity.EventReference fistSwingEvent;

    private Movement playerMovement;
    private Player playerScript;

    [SerializeField]
    private Rigidbody playerRb;

    void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        playerAttack = new PlayerInput();
        playerAttack.Enable();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<Movement>();
        playerScript = player.GetComponent<Player>();
        playerRb = player.GetComponent<Rigidbody>();
    }

    void OnEnable()
    {

        playerAttack.Player_Map.Attack.performed += Attack_M1;
        playerAttack.Player_Map.SpecialAttack.performed += Attack_M2;
    }

    void OnDisable()
    {

        playerAttack.Player_Map.Attack.performed -= Attack_M1;
        playerAttack.Player_Map.SpecialAttack.performed -= Attack_M2;
    }


    private IEnumerator ApplyForwardLunge(float attackDelay)
    {
        //apply force forwards
        playerMovement.isAnimLocked = true;
        yield return new WaitForSeconds(attackDelay);
        playerRb.velocity = new Vector3(transform.forward.x, 0, transform.forward.z) * smallSwingLungeForce;
        yield return new WaitForSeconds(smallSwingLungeTime);
        playerRb.velocity = Vector3.zero;
        playerMovement.isAnimLocked = false;
        ResetAttackPhase();

    }


    private IEnumerator ResetDefendLockIn(float defendCooldown)
    {
        yield return new WaitForSeconds(defendCooldown);
        playerScript.setInvincible(false);
        ResetAttackPhase();
    }

    private void ResetAttackPhase()
    {
        animator.SetInteger("attack", 0);
        isAttacking = false;
    }

    public override void Attack_M1(CallbackContext context)
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetInteger("attack", 1);

        // play fist swing event
        //FMODUnity.RuntimeManager.PlayOneShot(fistSwingEvent);

        StartCoroutine(ResetAttackLockIn(smallAttackCooldown));
        StartCoroutine(ApplyForwardLunge(smallSwingAttackDelay));
        StartCoroutine(AttackRaycast(smallSwingAttackRange, smallSwingAttackDamage, smallSwingAttackDelay));
    }

    public override void Attack_M2(CallbackContext context)
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetInteger("attack", 2);
        playerScript.setInvincible(true);

        //FMODUnity.RuntimeManager.PlayOneShot(defendEvent);

        StartCoroutine(ResetDefendLockIn(defendCooldown));
    }
}
