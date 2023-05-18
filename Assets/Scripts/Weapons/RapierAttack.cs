using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapierAttack : MonoBehaviour
{
    [SerializeField]
    private float M1AttackRange = 3f;

    [SerializeField]
    private float M2AttackDelay = 1f;

    [SerializeField]
    private float M1AttackDelay = 1f;

    [SerializeField]
    private float M1AttackDamage = 1f;
    [SerializeField]
    private float M2AttackDamage = 2f;

    [SerializeField]
    private float M2LungeForce = 100f;

    [SerializeField]
    private float M2LungeTime = 1f;

    [SerializeField]
    private float M2AttackRange = 15f;

    [SerializeField]
    private float M2AttackCooldown = 3f;

    private float M2attackCooldownCounter = 0;

    // create fmod references
    public FMODUnity.EventReference rapierSwingEvent;
    public FMODUnity.EventReference fistHitEvent;

    public LayerMask attackLayer;
    Camera cam;
    Animator animator;
    public GameObject hitEffect;

    [SerializeField]
    private Rigidbody playerRb;

    private float attackTimer;

    private PlayerInput playerAttack;
    string currentAnimationState;

    public const string IDLE = "Rapier_Idle";
    public const string LUNGE = "Armature_Lunge";

    public const string DOUBLE_LUNGE = "Armature_DoubleLunge";

    private const float COMBO_MAX_DELAY = 0.5f;

    private static int no_of_clicks = 0;
    private float lastClickedTime = 0;
    private bool isAttacking = false;
    private bool readyToLunge = true;


    private Movement playerMovement;

    void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        playerAttack = new PlayerInput();
        playerAttack.Enable();
        //get player rigidbody
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<Movement>();
        playerRb = player.GetComponent<Rigidbody>();


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

    private void Update()
    {
        if (playerAttack.Player_Map.Attack.IsPressed())
        {
            Attack();
        }
        if (playerAttack.Player_Map.SpecialAttack.IsPressed())
        {
            SpecialAttack();
        }
        HandleLungeCoolDown();
        ResetCombo();



    }
    void HitTarget(Vector3 pos, GameObject hittable)
    {
        // play fist hit event - to do: change to rapier hit event
        FMODUnity.RuntimeManager.PlayOneShot(fistHitEvent);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        GO.transform.parent = hittable.transform;
        Destroy(GO, 20);
    }
    private IEnumerator ResetAttackLockIn(float attackCooldown)
    {
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
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


    private void ResetCombo()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Lunge 1"))
        {

            animator.SetBool("first_combo_step", false);

        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Lunge 2"))
        {

            animator.SetBool("second_combo_step", false);

        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Double Lunge"))
        {

            animator.SetBool("third_combo_step", false);
            no_of_clicks = 0;

        }

        if (Time.time - lastClickedTime > COMBO_MAX_DELAY)
        {

            no_of_clicks = 0;
        }
    }

    private void Attack()
    {
        if (isAttacking) return;

        lastClickedTime = Time.time;
        no_of_clicks++;
        if (no_of_clicks == 1)
        {
            animator.SetBool("first_combo_step", true);
            animator.SetBool("third_combo_step", false);
            animator.SetBool("second_combo_step", false);
            StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay));
        }
        if (no_of_clicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Lunge 1"))
        {
            StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay));
            animator.SetBool("first_combo_step", false);
            animator.SetBool("third_combo_step", false);
            animator.SetBool("second_combo_step", true);

        }
        if (no_of_clicks >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Lunge 2"))
        {
            StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay / 2));
            StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay));
            animator.SetBool("second_combo_step", false);
            animator.SetBool("first_combo_step", false);
            animator.SetBool("third_combo_step", true);
        }
        no_of_clicks = Mathf.Clamp(no_of_clicks, 0, 3);


        // play rapier swing event
        //FMODUnity.RuntimeManager.PlayOneShot(rapierSwingEvent);

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

    }

    private void SpecialAttack()
    {
        if (isAttacking || !readyToLunge) return;

        readyToLunge = false;
        M2attackCooldownCounter = M2AttackCooldown;

        isAttacking = true;

        animator.SetBool("first_combo_step", true);

        // play rapier swing event
        //FMODUnity.RuntimeManager.PlayOneShot(rapierSwingEvent);

        StartCoroutine(ResetAttackLockIn(M2AttackCooldown));
        StartCoroutine(ApplyForwardLunge(M2AttackDelay));
        StartCoroutine(AttackRaycast(M2AttackRange, M2AttackDamage, M2AttackDelay));

    }


}
