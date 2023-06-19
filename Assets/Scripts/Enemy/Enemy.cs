using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public GameObject bullet;
    public Transform bulletPoint;


    private Transform playerTransform;
    private NavMeshAgent agent;
    Animator animator;
    public float attackRange = 3;
    public int chaseRange = 10;


    /*private float playerDetectionRange = 10.0f;

    private bool targetingPlayer = false;
   private bool isAttacking = false;
    private float attackStart = 0;*/

    // Start is called before the first frame update
    void Start()
    {
        /*agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Vector3.Distance(playerTransform.position, transform.position) <= attackRange)
        {
            Attack();
        } 

        if (Time.time - attackStart >= 3)
        {
            //animator.SetBool("isAttacking", false);
            //isAttacking = false;
        }
        
        
        if (!isAttacking && (targetingPlayer || Vector3.Distance(playerTransform.position, transform.position) <= playerDetectionRange))
        {
            targetingPlayer = true;
            agent.SetDestination(playerTransform.position);
            Walk();
        }*/
    }

    void Walk()
    {
        //animator.SetBool("isWalking", true);
    }

    void Attack()
    {
        /*animator.SetBool("isWalking", false);
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        attackStart = Time.time;*/
    }

    public void Shoot()
    {
        Rigidbody rb = Instantiate(bullet, bulletPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        //rb.AddForce(transform.up * 7, ForceMode.Impulse);
    }

    public void SniperShoot()
    {
        Rigidbody rb = Instantiate(bullet, bulletPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        //rb.AddForce(transform.up * 7, ForceMode.Impulse);
    }

    public void ShootgunShoot()
    {
        Vector3 bulletPos = bulletPoint.position;
        Rigidbody r1 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r1.AddForce(transform.forward * 50f, ForceMode.Impulse);
        bulletPos.y += 0.05f;
        Rigidbody r2 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r2.AddForce(transform.forward * 50f + transform.up * 10f, ForceMode.Impulse);
        bulletPos.y += -0.1f;
        Rigidbody r3 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r3.AddForce(transform.forward * 50f + transform.up * -10f, ForceMode.Impulse);
        bulletPos.y += 0.05f;
        bulletPos.x += 0.05f;
        Rigidbody r4 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r4.AddForce(transform.forward * 50f + transform.right * 10f, ForceMode.Impulse);
        bulletPos.x += -0.1f;
        Rigidbody r5 = Instantiate(bullet, bulletPos, Quaternion.identity).GetComponent<Rigidbody>();
        r5.AddForce(transform.forward * 50f + transform.right * -10f, ForceMode.Impulse);

        Destroy(r1, 1);
        Destroy(r2, 1);
        Destroy(r3, 1);
        Destroy(r4, 1);
        Destroy(r5, 1);
    }

    override protected void Death()
    {
        Destroy(gameObject, 0.1f);
    }
}
