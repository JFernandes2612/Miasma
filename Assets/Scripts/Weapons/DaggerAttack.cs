using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DaggerAttack : Weapon
{
    // M2 PARAMETERS
    [SerializeField]
    private float M2AttackDamage = 1000f; //execute attack

    [SerializeField]
    private float M2ExecuteRange = 15f;

    private float M2AttackRange = 4f;

    [SerializeField]
    private float M2AttackDelay = 0.8f;

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

    [SerializeField]
    private float M1MaxChargeUpTime = 2f;

    [SerializeField]
    private float M1ProjectileSpeed = 3f;

    public GameObject daggerProjectile;

    private Movement playerMovement;

    private Look playerLook;

    private Look noPosEffectLook;

    private float countChargeUpTime = 0f;

    private bool canStartCharge = false;

    private const string IDLE = "Idle";

    public override  float getRMBCooldown(){
        return M2attackCooldownCounter/ M2AttackCooldown;
    }

    public float getChargeUp(){
        return countChargeUpTime/M1MaxChargeUpTime;
    }

    public bool isCharging(){
        return canStartCharge;
    }

    public override  float getLMBCooldown(){
        return M1AttackDelay;
    }

    public override  bool isRMBCooldown(){
        return !readyToM2;
    }

    public override  bool isLMBCooldown(){
        return isAttacking && !canStartCharge;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        playerAttack = new PlayerInput();
        playerAttack.Enable();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerLook = cam.GetComponent<Look>();
        noPosEffectLook = GameObject.Find("WeaponCameraNoPosEffects").GetComponent<Look>();
        playerMovement = player.GetComponent<Movement>();
    }

    void HandleDaggerCharge()
    {
        if (playerAttack.Player_Map.Attack.IsPressed() && canStartCharge)
        {
            countChargeUpTime += Time.deltaTime;
            return;
        }
        countChargeUpTime = 0f;

    }

    void HandleAssassinationCooldown()
    {
        if (M2attackCooldownCounter > 0f)
        {
            M2attackCooldownCounter -= Time.deltaTime;
            return;
        }
        readyToM2 = true;
    }

    void Update()
    {
        HandleDaggerCharge();
        HandleAssassinationCooldown();

    }
    void OnEnable()
    {

        playerAttack.Player_Map.Attack.started += Attack_M1;
        playerAttack.Player_Map.Attack.performed += Attack_M1;
        playerAttack.Player_Map.Attack.canceled += Attack_M1;
        playerAttack.Player_Map.SpecialAttack.performed += Attack_M2;
    }

    void OnDisable()
    {

        playerAttack.Player_Map.Attack.started -= Attack_M1;
        playerAttack.Player_Map.Attack.performed -= Attack_M1;
        playerAttack.Player_Map.Attack.canceled -= Attack_M1;
        playerAttack.Player_Map.SpecialAttack.performed -= Attack_M2;
    }
    IEnumerator SpawnDaggerProjectiles(int numDaggersToSpawn, Vector3 spawnPos, Quaternion currentRotation)
    {
        for (int i = 0; i < numDaggersToSpawn; i++)
        {
            yield return new WaitForSeconds(M1AttackDelay);
            GameObject dagger = Instantiate(daggerProjectile, spawnPos, currentRotation);
            dagger.GetComponent<DaggerProjectile>().SetDamage(M1AttackDamage);
            dagger.GetComponent<DaggerProjectile>().SetDirection(transform.forward);
            dagger.GetComponent<DaggerProjectile>().SetSpeed(M1ProjectileSpeed);
            isAttacking = false;
        }
    }


    public override void Attack_M1(InputAction.CallbackContext context)
    {


        if (context.started)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(IDLE) || canStartCharge) return;
            canStartCharge = true;
            playerMovement.SlowPlayer();
            // start charging up attack
            animator.SetBool("isCharging", true);

        }
        else if (context.canceled)
        {

            if (!canStartCharge) return;
            canStartCharge = false;

            isAttacking = true;

            playerMovement.UnSlowPlayer();

            // release attack
            animator.SetBool("isCharging", false);

            //daggerReleaseEvent.Play();
            // spawn daggers and send them flying forwards

            int numDaggersToSpawn = countChargeUpTime > M1MaxChargeUpTime ? 3 : 1;

            float compensationHeight = 1f;
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + compensationHeight, transform.position.z);

            Quaternion newRotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            StartCoroutine(SpawnDaggerProjectiles(numDaggersToSpawn, spawnPos, newRotation));

        }
    }

    IEnumerator performExecute(float executeDelay, Enemy enemy)
    {
        playerLook.LockCamera();
        noPosEffectLook.LockCamera();
        playerMovement.isAnimLocked = true;
        yield return new WaitForSeconds(executeDelay);

        playerMovement.transform.position = enemy.transform.position - enemy.transform.forward * 1.5f;
        playerMovement.transform.rotation = Quaternion.LookRotation(enemy.transform.forward);

        if (Physics.Raycast(cam.transform.position,
        cam.transform.forward, out RaycastHit hit, M2AttackRange, attackLayer))
        {
            if (hit.transform.TryGetComponent<Enemy>(out Enemy T))
            {
                HitTarget(hit.point, T.gameObject);

            }
        }
        enemy.TakeDamage(M2AttackDamage);
        playerLook.UnlockCamera();
        noPosEffectLook.UnlockCamera();
        playerMovement.isAnimLocked = false;


    }

    public override void Attack_M2(InputAction.CallbackContext context)
    {
        //check if an enemy is in the mouse position
        if (context.performed)
        {

            if (!readyToM2 || !animator.GetCurrentAnimatorStateInfo(0).IsName(IDLE)) return;
            

            //play assassination sound
            //FMODUnity.RuntimeManager.PlayOneShot(M2AttackEvent);

            //raycast to check if enemy is in front of player
            if (Physics.Raycast(cam.transform.position,
            cam.transform.forward, out RaycastHit hit, M2ExecuteRange, attackLayer))
            {
                if (hit.transform.TryGetComponent<Enemy>(out Enemy T))
                {
                    readyToM2 = false;
                    M2attackCooldownCounter = M2AttackCooldown;
                    isAttacking = true;

                    //teleport to behind enemy
                    T.Freeze();
                    animator.SetTrigger("backStab");

                    StartCoroutine(performExecute(M2AttackDelay, T));

                }
            }
        }
    }
}
