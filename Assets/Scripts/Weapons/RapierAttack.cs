using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class RapierAttack : MonoBehaviour
{
    // M2 PARAMETERS
    [SerializeField]
    private float M2AttackDamage = 2f;

    [SerializeField]
    private float M2AttackDelay = 1f;

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

    [SerializeField]
    private float M1AttackDamage = 1f;

    public LayerMask attackLayer;
    public Animator animator;
    Camera cam;

    private const string IDLE = "Idle";
    private const string LUNGE_1 = "Lunge 1";
    private const string LUNGE_2 = "Lunge 2";
    private const string DOUBLE_LUNGE = "Double Lunge";

    public GameObject hitEffect;
    //public FMODUnity.EventReference fistSwingEvent;

    private bool isAttacking = false;
    private bool readyToLunge = true;
    private PlayerInput playerAttack;
    private int CountAttack;

    private Movement playerMovement;

    [SerializeField]
    private Rigidbody playerRb;
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


    }
    void OnEnable()
    {
        Debug.Log("Enabled Rapier");
        playerAttack.Player_Map.Attack.performed += Attack_L1;
        playerAttack.Player_Map.SpecialAttack.performed += SpecialAttack;
    }

    void OnDisable()
    {
        Debug.Log("Disabled Rapier");
        playerAttack.Player_Map.Attack.performed -= Attack_L1;
        playerAttack.Player_Map.SpecialAttack.performed -= SpecialAttack;
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
            readyToLunge = true;

        }
    }

    public void M1AttackHitBox()
    {
        StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay));
    }
    private void Update()
    {
        if (CountAttack == 1)
        {
            animator.SetInteger("attackPhase", 1);
            //StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay));
            isAttacking = true;
        }
        HandleLungeCoolDown();

    }

    void HitTarget(Vector3 pos, GameObject hittable)
    {
        // play fist hit event
        // FMODUnity.RuntimeManager.PlayOneShot(fistHitEvent);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        GO.transform.parent = hittable.transform;
        Destroy(GO, 20);
    }

    private IEnumerator AttackRaycast(float attackRange, float attackDamage, float attackDelay)
    {

        yield return new WaitForSeconds(attackDelay);
        if (Physics.Raycast(cam.transform.position,
        cam.transform.forward, out RaycastHit hit, attackRange, attackLayer))
        {
            if (hit.transform.TryGetComponent<Entity>(out Entity T))
            {
                HitTarget(hit.point, T.gameObject);
                T.TakeDamage(attackDamage);
            }
        }
    }

    private void Attack_L1(CallbackContext context)
    {

        CountAttack++;

        // play fist swing event
        //FMODUnity.RuntimeManager.PlayOneShot(fistSwingEvent);

    }

    private IEnumerator ResetAttackLockIn(float attackCooldown)
    {
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    private void SpecialAttack(CallbackContext context)
    {
        if (isAttacking || !readyToLunge) return;

        readyToLunge = false;
        M2attackCooldownCounter = M2AttackCooldown;

        isAttacking = true;

        animator.SetInteger("attackPhase", 1);

        // play rapier swing event
        //FMODUnity.RuntimeManager.PlayOneShot(rapierSwingEvent);

        StartCoroutine(ResetAttackLockIn(M2AttackCooldown));
        StartCoroutine(ApplyForwardLunge(M2AttackDelay));
        StartCoroutine(AttackRaycast(M2AttackRange, M2AttackDamage, M2AttackDelay));

    }

    public void CheckAttackPhase()
    {
        Debug.Log("CountAttack: " + CountAttack);
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

}
