using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapierAttack : MonoBehaviour
{
    [SerializeField]
    private float attackRange = 3f;

    [SerializeField]
    private float attackDelay = 0.5f;

    [SerializeField]
    private float attackDamage = 1f;

    [SerializeField]
    private float M1attackCooldown = 0.2f; // attack Speed

    // create fmod references
    public FMODUnity.EventReference rapierSwingEvent;
    public FMODUnity.EventReference fistHitEvent;

    public LayerMask attackLayer;
    Camera cam;
    Animator animator;
    public GameObject hitEffect;

    private float attackTimer;

    private PlayerInput playerAttack;
    string currentAnimationState;

    public const string IDLE = "Rapier_Idle";
    public const string LUNGE = "Armature_Lunge";

    public const string DOUBLE_LUNGE = "Armature_DoubleLunge";

    private const float COMBO_MAX_DELAY = 0.5f;


    private static int no_of_clicks = 0;
    private float lastClickedTime = 0;

    private float nextFireTime = 0;
    private bool isAttacking;
    private bool readyToAttack;

    void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        playerAttack = new PlayerInput();
        playerAttack.Enable();


    }


    public void ChangeAnimationState(string newState)
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        //if (currentAnimationState == newState) return;

        // PLAY THE ANIMATION //
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }

    private void Update()
    {
        if (playerAttack.Player_Map.Attack.IsPressed())
        {

            Attack();
        }
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
    void ResetAttack()
    {
        isAttacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
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
        lastClickedTime = Time.time;
        no_of_clicks++;
        if (no_of_clicks == 1)
        {
            animator.SetBool("first_combo_step", true);
        }
        if (no_of_clicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Lunge 1"))
        {
            animator.SetBool("first_combo_step", false);
            animator.SetBool("second_combo_step", true);
        }
        if (no_of_clicks >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Lunge 2"))
        {
            animator.SetBool("second_combo_step", false);
            animator.SetBool("third_combo_step", true);
        }
        no_of_clicks = Mathf.Clamp(no_of_clicks, 0, 3);


        // play rapier swing event
        //FMODUnity.RuntimeManager.PlayOneShot(rapierSwingEvent);

    }


}
