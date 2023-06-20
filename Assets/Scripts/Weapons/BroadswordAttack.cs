using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class BroadswordAttack : MonoBehaviour
{
    // For both attacks
    [SerializeField]
    private float attackCooldown = 3.2f;
    [SerializeField]
    private float attackDamage = 5.0f;
    [SerializeField]
    private float attackRange = 3f;

    // First attack
    [SerializeField]
    private float smallSwingAttackDelay = 1.1f; // Time between the input action and applying the lunge force
    [SerializeField]
    private float smallSwingLungeTime = 3.0f; // Time the lunge force is applied for
    [SerializeField]
    private float smallSwingLungeForce = 20f; // Lunge force

    // Second attack
    [SerializeField]
    private float upperSwingAttackDelay = 1.1f; // Time between the input action and applying the lunge force
    [SerializeField]
    private float upperSwingLungeTime = 5.0f; // Time the lunge force is applied for
    [SerializeField]
    private float upperSwingLungeForce = 30f; // Lunge force

    [SerializeField]
    private float defendDuration = 2.0f; // Invincibility duration (animation is reset at the end)

    public LayerMask attackLayer;
    public Animator animator;
    Camera cam;

    public GameObject hitEffect;
    //public FMODUnity.EventReference fistSwingEvent;

    private bool isAttacking = false; // tells us if we're locked in an attack/defense animation
    private PlayerInput playerAttack;
    private int CountAttack; // 0 or 1 (first attack or second attack)

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
        CountAttack = 0;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<Movement>();
        playerScript = player.GetComponent<Player>();
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

    private void SpecialAttack(CallbackContext context)
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetInteger("attack", 3);
        playerScript.setInvincible(true);

        //FMODUnity.RuntimeManager.PlayOneShot(defendEvent);

        StartCoroutine(ResetDefendLockIn(defendDuration));
    }

    private void Attack(CallbackContext context)
    {
        if (isAttacking) return;

        isAttacking = true;
        Vector3 lungeDirection;
        // lunge direction changes depending on the attack, and so does the lunge time, delay and force
        if (CountAttack == 0)
        {
            animator.SetInteger("attack", 1);
            lungeDirection = new Vector3(transform.forward.x, 0, transform.forward.z);
            StartCoroutine(ApplyLunge(smallSwingAttackDelay, smallSwingLungeTime, smallSwingLungeForce, lungeDirection));
        }
        else if (CountAttack == 1)
        {
            animator.SetInteger("attack", 2);
            lungeDirection = new Vector3(0, 1, transform.forward.z);
            StartCoroutine(ApplyLunge(upperSwingAttackDelay, upperSwingLungeTime, upperSwingLungeForce, lungeDirection));
        }

        // play fist swing event
        //FMODUnity.RuntimeManager.PlayOneShot(fistSwingEvent);

        CountAttack = (CountAttack + 1) % 2;
        StartCoroutine(ResetAttackLockIn(attackCooldown));
        StartCoroutine(AttackRaycast(attackRange, attackDamage, smallSwingAttackDelay));
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

    private IEnumerator AttackRaycast(float attackRange, float attackDamage, float attackDelay)
    {
        // wait until just after the lunge
        yield return new WaitForSeconds(attackDelay+0.01f);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackRange, attackLayer))
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
        // FMODUnity.RuntimeManager.PlayOneShot(fistHitEvent);
        if(hitEffect){
            GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
            GO.transform.parent = hittable.transform;
            Destroy(GO, 20);
        }
        else{
            Debug.LogWarning("Missing hit decal");
        }
    }

    private IEnumerator ResetDefendLockIn(float defendDuration)
    {
        yield return new WaitForSeconds(defendDuration);
        playerScript.setInvincible(false);
        ResetAttackPhase();
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
