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

    private const string IDLE = "Idle";
    private const string LUNGE_1 = "Lunge 1";
    private const string LUNGE_2 = "Lunge 2";
    private const string DOUBLE_LUNGE = "Double Lunge";


    [SerializeField]
    private float COMBO_MAX_DELAY = 0.5f;

    private static int comboStep = 0;
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
    public void ChangeAnimationState(string newState)
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        if (currentAnimationState == newState) return;
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }

    private void ResetCombo()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(DOUBLE_LUNGE) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f) comboStep = 0;
        if (Time.time - lastClickedTime > COMBO_MAX_DELAY)
        {
            Debug.Log("Combo Reset");
            comboStep = 0;
        }
    }

    private void Attack()
    {
        if (isAttacking) return;

        lastClickedTime = Time.time;
        comboStep++;
        if (comboStep == 1)
        {
            ChangeAnimationState(LUNGE_1);
            StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay));
            Debug.Log("first attack");
        }
        if (comboStep >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName(LUNGE_1))
        {

            ChangeAnimationState(LUNGE_2);
            Debug.Log("second attack");
            StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay));

        }
        if (comboStep >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName(LUNGE_2))
        {
            ChangeAnimationState(DOUBLE_LUNGE);
            StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay / 2));
            StartCoroutine(AttackRaycast(M1AttackRange, M1AttackDamage, M1AttackDelay));
            Debug.Log("third attack");

        }
        comboStep = Mathf.Clamp(comboStep, 0, 3);


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

        animator.SetTrigger("FirstComboStep");

        // play rapier swing event
        //FMODUnity.RuntimeManager.PlayOneShot(rapierSwingEvent);

        StartCoroutine(ResetAttackLockIn(M2AttackCooldown));
        StartCoroutine(ApplyForwardLunge(M2AttackDelay));
        StartCoroutine(AttackRaycast(M2AttackRange, M2AttackDamage, M2AttackDelay));

    }


}
