using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class BroadswordAttack : MonoBehaviour
{
    [SerializeField]
    private float attackCooldown = 3.2f;
    [SerializeField]
    private float attackDamage = 5.0f;
    [SerializeField]
    private float attackRange = 3f;

    [SerializeField]
    private float smallSwingAttackDelay = 1.1f;
    [SerializeField]
    private float smallSwingLungeTime = 3.0f;
    [SerializeField]
    private float smallSwingLungeForce = 20f;

    [SerializeField]
    private float upperSwingAttackDelay = 1.1f;
    [SerializeField]
    private float upperSwingLungeTime = 5.0f;
    [SerializeField]
    private float upperSwingLungeForce = 30f;

    [SerializeField]
    private float defendDuration = 2.0f;

    public LayerMask attackLayer;
    public Animator animator;
    Camera cam;

    public GameObject hitEffect;
    //public FMODUnity.EventReference fistSwingEvent;

    private bool isAttacking = false;
    private PlayerInput playerAttack;
    private int CountAttack;

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
        // wait just until after the lunge
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
