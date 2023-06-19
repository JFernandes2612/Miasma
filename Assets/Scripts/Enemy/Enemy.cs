using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    private Transform playerTransform;
    private NavMeshAgent agent;

    private float playerDetectionRange = 10.0f;

    private bool targetingPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetingPlayer || Vector3.Distance(playerTransform.position, transform.position) <= playerDetectionRange)
        {
            targetingPlayer = true;
            agent.SetDestination(playerTransform.position);
        }
    }

    public void Freeze()
    {
        agent.isStopped = true;
    }

    override protected void Death()
    {
        Destroy(gameObject, 0.1f);
    }
}
