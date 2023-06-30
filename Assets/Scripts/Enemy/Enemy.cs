using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public GameObject bullet;
    public Transform bulletPoint;
    public ParticleSystem shotEffectPrefab;
    public int extraDamage = 0;

    public float bulletVelocity = 2f;

    public float playerExtraHealthOnKill = 100f;
    private Transform playerTransform;
    private NavMeshAgent agent;
    public Animator animator;
    public float attackRange = 3;
    public int chaseRange = 10;

    private Vector3 directionToPlayer;

    public List<Transform> wayPoints = new List<Transform>();

    float timer;

    float prevAnimatorSpeed;
    float prevAgentSpeed;

    public FMODUnity.EventReference lightsaberEffect;
    private FMOD.Studio.EventInstance lightsaberInstance;
    public FMODUnity.EventReference raygunEffect;
    private FMOD.Studio.EventInstance raygunInstance;
    public FMODUnity.EventReference rayshotgunEffect;
    private FMOD.Studio.EventInstance rayshotgunInstance;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        directionToPlayer = (playerTransform.position - transform.position).normalized;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        prevAnimatorSpeed = animator.speed;
        prevAgentSpeed = agent.speed;

        lightsaberInstance = FMODUnity.RuntimeManager.CreateInstance(lightsaberEffect);
        raygunInstance = FMODUnity.RuntimeManager.CreateInstance(raygunEffect);
        rayshotgunInstance = FMODUnity.RuntimeManager.CreateInstance(rayshotgunEffect);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        directionToPlayer = (playerTransform.position - transform.position).normalized;
        directionToPlayer.y -= directionToPlayer.y * 1f;

        if (timer > 5 && currentHealth > 0)
        {
            timer = 0;
            //TakeDamage(101);
            //Freeze();
        }
    }

    public override void TakeDamage(float damageAmount)
    {
        //Play Enemy Getting Hit Sound

        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            animator.SetTrigger("isDying");
            GetComponent<CapsuleCollider>().enabled = false;
            Death();
        }
        else
        {
            animator.SetTrigger("isHit");
        }
    }

    public void Shoot()
    {
        //Play PistolShoot Sound

        Rigidbody rb = Instantiate(bullet, bulletPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(directionToPlayer * bulletVelocity, ForceMode.Impulse);
        Destroy(rb, 3);

        ParticleSystem shotEffect = Instantiate(shotEffectPrefab, bulletPoint.position, Quaternion.identity);
        shotEffect.Play();
        Destroy(shotEffect.gameObject, 0.1f);
    }

    public void SniperShoot()
    {
        //Play SniperShoot Sound

        Rigidbody rb = Instantiate(bullet, bulletPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(directionToPlayer * bulletVelocity, ForceMode.Impulse);
        Destroy(rb, 3);

        ParticleSystem shotEffect = Instantiate(shotEffectPrefab, bulletPoint.position, Quaternion.identity);
        shotEffect.Play();
        Destroy(shotEffect.gameObject, 0.1f);
    }

    public void ShootgunShoot()
    {

        //Play ShootgunShoot Sound

        Vector3 bulletPos = bulletPoint.position;
        Rigidbody r1 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r1.AddForce(directionToPlayer * bulletVelocity, ForceMode.Impulse);
        bulletPos.y += 0.1f;
        Rigidbody r2 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r2.AddForce(directionToPlayer * bulletVelocity + transform.up * bulletVelocity/10, ForceMode.Impulse);
        bulletPos.y += -0.2f;
        Rigidbody r3 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r3.AddForce(directionToPlayer * bulletVelocity + transform.up * -bulletVelocity/10, ForceMode.Impulse);
        bulletPos.y += 0.1f;
        bulletPos.x += 0.1f;
        Rigidbody r4 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r4.AddForce(directionToPlayer * bulletVelocity + transform.right * bulletVelocity/10, ForceMode.Impulse);
        bulletPos.x += -0.2f;
        Rigidbody r5 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r5.AddForce(directionToPlayer * bulletVelocity + transform.right * -bulletVelocity/10, ForceMode.Impulse);

        Destroy(r1, 3);
        Destroy(r2, 3);
        Destroy(r3, 3);
        Destroy(r4, 3);
        Destroy(r5, 3);

        ParticleSystem shotEffect = Instantiate(shotEffectPrefab, bulletPoint.position, Quaternion.identity);
        shotEffect.Play();
        Destroy(shotEffect.gameObject, 0.1f);
    }

    public void Freeze()
    {
        PauseAnimation();
    }

    public void UnFreeze()
    {
        ResumeAnimation();
    }

    override protected void Death()
    {
        //Play Enemy Dying Sound
        playerTransform.gameObject.GetComponent<Player>().GetHealthBack(playerExtraHealthOnKill);
        Destroy(gameObject, 5);
    }

    private void PauseAnimation()
    {

        agent.speed = 0f;

        agent.angularSpeed = 0f;

        animator = GetComponent<Animator>();
        prevAnimatorSpeed = animator.speed;
        prevAgentSpeed = agent.speed;

        // Pause the animation
        animator.speed = 0f;



    }

    private void ResumeAnimation()
    {
        // Resume the animation
        animator.speed = prevAnimatorSpeed;
        agent.speed = prevAgentSpeed;
    }

    public void PlayLightsaberEffect()
    {
        lightsaberInstance.start();
    }

    public void PlayRaygunEffect()
    {
        raygunInstance.start();
    }

    public void PlayRayshotgunEffect()
    {
        rayshotgunInstance.start();
    }
}
