using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class BroadswordAttack : MonoBehaviour
{
    [SerializeField]
    private float smallAttackCooldown = 2.3f;
    [SerializeField]
    private float smallSwingLungeDelay = 1.1f;
    [SerializeField]
    private float smallSwingLungeTime = 0.5f;
    [SerializeField]
    private float smallSwingLungeForce = 10f;
    [SerializeField]
    private float smallSwingAttackDamage = 5.0f;

    [SerializeField]
    private float defendCooldown = 10f;

    public LayerMask attackLayer;
    public Animator animator;
    Camera cam;

    public GameObject hitEffect;
    //public FMODUnity.EventReference fistSwingEvent;

    private bool isAttacking = false;
    private PlayerInput playerAttack;

    private Movement playerMovement;

    [SerializeField]
    private Rigidbody playerRb;

    // check these below

    [SerializeField]
    private float AttackRange = 2f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        playerAttack = new PlayerInput();
        playerAttack.Enable();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<Movement>();
        playerRb = player.GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        Debug.Log("Enabled Broadsword");
        playerAttack.Player_Map.Attack.performed += Attack;
        playerAttack.Player_Map.SpecialAttack.performed += SpecialAttack;
    }

    void OnDisable()
    {
        Debug.Log("Disabled Broadsword");
        playerAttack.Player_Map.Attack.performed -= Attack;
        playerAttack.Player_Map.SpecialAttack.performed -= SpecialAttack;
    }

    public void AttackHitBox()
    {
        // StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay));
    }

    private void Update()
    {
        // if (CountAttack == 1)
        // {
        //     animator.SetInteger("attackPhase", 1);
        //     //StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay));
        //     isAttacking = true;
        // }
    }

    private void SpecialAttack(CallbackContext context)
    {
        if (isAttacking) return;

        isAttacking = true;

        Debug.Log("Defend not implemented");

        // animator.SetInteger("attackPhase", 2);

        //FMODUnity.RuntimeManager.PlayOneShot(defendEvent);

        StartCoroutine(ResetAttackLockIn(defendCooldown));
    }

    private void Attack(CallbackContext context)
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetInteger("attack", 1);

        // play fist swing event
        //FMODUnity.RuntimeManager.PlayOneShot(fistSwingEvent);

        StartCoroutine(ResetAttackLockIn(smallAttackCooldown));
        StartCoroutine(ApplyForwardLunge(smallSwingLungeDelay, smallSwingLungeTime, smallSwingLungeForce));
    }
    
    private IEnumerator ApplyForwardLunge(float attackDelay, float lungeTime, float lungeForce)
    {
        //apply force forwards
        yield return new WaitForSeconds(attackDelay);
        playerRb.velocity = new Vector3(transform.forward.x, 0, transform.forward.z) * lungeForce;
        yield return new WaitForSeconds(lungeTime);
        playerRb.velocity = Vector3.zero;
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

    void HitTarget(Vector3 pos, GameObject hittable)
    {
        // play fist hit event
        // FMODUnity.RuntimeManager.PlayOneShot(fistHitEvent);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        GO.transform.parent = hittable.transform;
        Destroy(GO, 20);
    }

    private IEnumerator ResetAttackLockIn(float attackCooldown)
    {
        yield return new WaitForSeconds(attackCooldown);
        ResetAttackPhase();
    }

    private void ResetAttackPhase()
    {
        animator.SetInteger("attack", 0);
        isAttacking = false;
    }
}
