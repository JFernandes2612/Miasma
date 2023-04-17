using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistAttack : MonoBehaviour
{
    [SerializeField]
    private float attackRange = 3f;

    [SerializeField]
    private float attackDelay = 0.5f;

    [SerializeField]
    private float attackDamage = 1f;

    [SerializeField]
    private float attackCooldown = 1f; // attack Speed

    AudioSource audioSource;
    public LayerMask attackLayer;
    Camera cam;
    Animator animator;
    public GameObject hitEffect;
    public AudioClip fistHitSound;
    public AudioClip fistSwingSound;

    private bool isAttacking = false;
    private bool readyToAttack = true;

    private float attackTimer;

    private PlayerInput playerAttack;
    string currentAnimationState;

    public const string IDLE = "IdlePunch";
    public const string RIGHT_PUNCH = "RightPunch";

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;
        playerAttack = new PlayerInput();
        playerAttack.Enable();
    }


    public void ChangeAnimationState(string newState)
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        if (currentAnimationState == newState) return;

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
        SetAnimations();
    }
    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(fistHitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }
    void ResetAttack()
    {
        isAttacking = false;
        readyToAttack = true;
    }
    void SetAnimations()
    {
        // If player is not attacking
        if (!isAttacking)
        {

            ChangeAnimationState(IDLE);

        }
    }
    void AttackRaycast()
    {
        if (Physics.Raycast(cam.transform.position,
        cam.transform.forward, out RaycastHit hit, attackRange, attackLayer))
        {
            HitTarget(hit.point);

            if (hit.transform.TryGetComponent<Enemy>(out Enemy T))
            { T.TakeDamage(attackDamage); }
        }
    }
    private void Attack()
    {

        if (!readyToAttack || isAttacking) return;

        readyToAttack = false;
        isAttacking = true;

        Invoke(nameof(ResetAttack), attackCooldown);
        Invoke(nameof(AttackRaycast), attackDelay);
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(fistSwingSound);
        ChangeAnimationState(RIGHT_PUNCH);
    }

}