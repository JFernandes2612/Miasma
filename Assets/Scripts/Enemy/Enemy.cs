using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    Animator animator;
    public int attackRange = 3;


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

    override protected void Death()
    {
        Destroy(gameObject, 0.1f);
    }
}
