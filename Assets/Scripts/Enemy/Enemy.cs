using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public int enemyHP = 100;
    public GameObject bullet;
    public Transform bulletPoint;


    private Transform playerTransform;
    private NavMeshAgent agent;
    public Animator animator;
    public float attackRange = 3;
    public int chaseRange = 10;

    private Vector3 directionToPlayer;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        directionToPlayer = (playerTransform.position - transform.position).normalized;
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
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damageAmount)
    {
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
        Rigidbody rb = Instantiate(bullet, bulletPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(directionToPlayer * 50f, ForceMode.Impulse);
        //rb.AddForce(transform.up * 7, ForceMode.Impulse);
    }

    public void SniperShoot()
    {

        Rigidbody rb = Instantiate(bullet, bulletPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(directionToPlayer * 50f, ForceMode.Impulse);
        //rb.AddForce(transform.up * 7, ForceMode.Impulse);
    }

    public void ShootgunShoot()
    {
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

        Destroy(r1, 2);
        Destroy(r2, 2);
        Destroy(r3, 2);
        Destroy(r4, 2);
        Destroy(r5, 2);
    }

    override protected void Death()
    {
        Destroy(gameObject, 10);
    }
}
