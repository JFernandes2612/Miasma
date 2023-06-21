using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public int enemyHP = 100;
    public GameObject bullet;
    public Transform bulletPoint;
    public ParticleSystem shotEffectPrefab;
    public int extraDamage = 0;

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
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        directionToPlayer = (playerTransform.position - transform.position).normalized;
        directionToPlayer.y += -0.1f;

        if (timer > 4 && enemyHP > 0)
        {
            timer = 0;
            //TakeDamage(20);
            //Freeze();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        //Play Enemy Getting Hit Sound

        enemyHP -= damageAmount;
        if(enemyHP <= 0)
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
        rb.AddForce(directionToPlayer * 50f, ForceMode.Impulse);
        Destroy(rb, 3);

        ParticleSystem shotEffect = Instantiate(shotEffectPrefab, bulletPoint.position, Quaternion.identity);
        shotEffect.Play();
        Destroy(shotEffect.gameObject, 0.1f);
    }

    public void SniperShoot()
    {
        //Play SniperShoot Sound

        Rigidbody rb = Instantiate(bullet, bulletPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(directionToPlayer * 50f, ForceMode.Impulse);
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
        r1.AddForce(directionToPlayer * 50f, ForceMode.Impulse);
        bulletPos.y += 0.05f;
        Rigidbody r2 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r2.AddForce(directionToPlayer * 50f + transform.up * 10f, ForceMode.Impulse);
        bulletPos.y += -0.1f;
        Rigidbody r3 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r3.AddForce(directionToPlayer * 50f + transform.up * -10f, ForceMode.Impulse);
        bulletPos.y += 0.05f;
        bulletPos.x += 0.05f;
        Rigidbody r4 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r4.AddForce(directionToPlayer * 50f + transform.right * 10f, ForceMode.Impulse);
        bulletPos.x += -0.1f;
        Rigidbody r5 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r5.AddForce(directionToPlayer * 50f + transform.right * -10f, ForceMode.Impulse);

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
        StartCoroutine(PauseAnimation());
    }

    override protected void Death()
    {
        //Play Enemy Dying Sound

        Destroy(gameObject, 10);
    }

    private IEnumerator PauseAnimation()
    {

        animator = GetComponent<Animator>();
        prevAnimatorSpeed = animator.speed;
        prevAgentSpeed = agent.speed;

        // Pause the animation
        animator.speed = 0f;
        agent.speed = 0f;

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Resume the animation
        animator.speed = prevAnimatorSpeed;
        agent.speed = prevAgentSpeed;
    }
}
