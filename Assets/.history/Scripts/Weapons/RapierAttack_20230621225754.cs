using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class RapierAttack : Weapon
{
    // M2 PARAMETERS
    [SerializeField]
    private float M2AttackDamage = 2f;

    [SerializeField]
    private float M2AttackDelay = 1.8f;

    [SerializeField]
    private float M2LungeForce = 100f;

    [SerializeField]
    private float M2LungeTime = 1f;

    [SerializeField]
    private float M2AttackRange = 15f;

    [SerializeField]
    private float M2AttackCooldown = 3f;

    private float M2attackCooldownCounter = 0f;

    // M1 PARAMETERS
    [SerializeField]
    private float M1AttackRange = 3f;

    [SerializeField]
    private float M1AttackDelay = 0.3f;

    private float M1CountDownTimer = 0f;
    private float M1AnimationDuration = 1.6f;
    [SerializeField]
    private float M1AttackDamage = 1f;

    private const string IDLE = "Idle";
    private const string LUNGE_1 = "Lunge 1";
    private const string LUNGE_2 = "Lunge 2";
    private const string DOUBLE_LUNGE = "Double Lunge";

    //public FMODUnity.EventReference fistSwingEvent;

    private BoxCollider rapierCollider;

    private int CountAttack;

    public FMODUnity.EventReference attackEffectsEvent; 
    private FMOD.Studio.EventInstance attackEffectsInstance;

    public int getAttackPhase()
    {
        return CountAttack;
    }

    private Movement playerMovement;

    [SerializeField]
    private Rigidbody playerRb;


    public override float getRMBCooldown()
    {
        return M2attackCooldownCounter / M2AttackCooldown;
    }

    public override float getLMBCooldown()
    {
        return M1CountDownTimer / M1AnimationDuration;
    }

    public override bool isRMBCooldown()
    {
        return !readyToM2;
    }

    public override bool isLMBCooldown()
    {
        return isAttacking;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        playerAttack = new PlayerInput();
        playerAttack.Enable();
        CountAttack = 0;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<Movement>();
        playerRb = player.GetComponent<Rigidbody>();
        rapierCollider = GameObject.Find("MarineRapier").GetComponent<BoxCollider>();


    }
    void OnEnable()
    {
        playerAttack.Player_Map.Attack.performed += Attack_M1;
        playerAttack.Player_Map.SpecialAttack.performed += Attack_M2;
    }

    void OnDisable()
    {

        disableRapierCollider();
        playerAttack.Player_Map.Attack.performed -= Attack_M1;
        playerAttack.Player_Map.SpecialAttack.performed -= Attack_M2;
    }

    public void enableRapierCollider()
    {
        rapierCollider.enabled = true;
    }

    public void disableRapierCollider()
    {
        rapierCollider.enabled = false;
    }

    private IEnumerator ApplyForwardLunge(float attackDelay)
    {
        //apply force forwards
        playerMovement.isAnimLocked = true;
        yield return new WaitForSeconds(attackDelay);
        playerRb.velocity = new Vector3(transform.forward.x, 0, transform.forward.z) * M2LungeForce;
        yield return new WaitForSeconds(M2LungeTime);
        playerRb.velocity = Vector3.zero;
        playerMovement.isAnimLocked = false;
        ResetAttackPhase();

    }
    private void HandleLungeCoolDown()
    {
        if (M2attackCooldownCounter > 0)
        {
            M2attackCooldownCounter -= Time.deltaTime;
        }
        else
        {
            readyToM2 = true;

        }
    }

    private void HandleM1CoolDown()
    {
        if (M1CountDownTimer > 0f)
        {
            M1CountDownTimer -= Time.deltaTime;
        }
        else
        {
            M1CountDownTimer = 0f;
        }


    }

    private void Update()
    {
        if (CountAttack == 1)
        {
            animator.SetInteger("attackPhase", 1);
            isAttacking = true;
        }
        HandleLungeCoolDown();
        HandleM1CoolDown();



    }


    public override void Attack_M1(CallbackContext context)
    {
        if (CountAttack < 3)
        {
            CountAttack++;
        }


    }


    public override void Attack_M2(CallbackContext context)
    {
        if (isAttacking || !readyToM2) return;

        readyToM2 = false;
        M2attackCooldownCounter = M2AttackCooldown;

        isAttacking = true;

        animator.SetInteger("attackPhase", 1);

        // play rapier swing event
        //FMODUnity.RuntimeManager.PlayOneShot(rapierSwingEvent);

        StartCoroutine(ResetAttackLockIn(M2AttackCooldown));
        StartCoroutine(ApplyForwardLunge(M2AttackDelay));
        StartCoroutine(AttackRaycast(M2AttackRange, M2AttackDamage, M2AttackDelay));

    }

    public void UpdateCountDownTimer()
    {
        isAttacking = true;
        M1CountDownTimer = M1AnimationDuration;
    }

    public void CheckAttackPhase()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(LUNGE_1))
        {
            if (CountAttack > 1)
            {
                animator.SetInteger("attackPhase", 2);

            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName(LUNGE_2))
        {
            if (CountAttack > 2)
            {
                animator.SetInteger("attackPhase", 3);

            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName(DOUBLE_LUNGE))
        {
            if (CountAttack >= 3)
            {
                ResetAttackPhase();
            }

        }

    }

    private void ResetAttackPhase()
    {
        animator.SetInteger("attackPhase", 0);
        CountAttack = 0;
        isAttacking = false;
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
