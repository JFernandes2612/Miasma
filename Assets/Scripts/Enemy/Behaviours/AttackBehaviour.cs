using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    float attackRange;
    float rayCastRange = 20;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackRange = animator.GetComponent<Enemy>().attackRange;
        agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 lookAtPosition = new Vector3(player.position.x, player.position.y - 1f, player.position.z);
        animator.transform.LookAt(lookAtPosition);
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance > attackRange*1.5)
            animator.SetBool("isAttacking", false);

        Vector3 direction = Vector3.forward;

        Vector3 newPosition = agent.transform.position;
        newPosition.y += 1f;

        Ray theRay = new Ray(newPosition, agent.transform.TransformDirection(direction * rayCastRange));
        //Debug.DrawLine(theRay.origin, theRay.origin + theRay.direction * rayCastRange, Color.red);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
