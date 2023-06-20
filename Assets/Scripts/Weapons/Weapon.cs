using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public abstract class Weapon : MonoBehaviour
{
    public GameObject hitEffect;
    public LayerMask attackLayer;
    protected Animator animator;

    protected bool isAttacking = false;

    protected PlayerInput playerAttack;
    protected bool readyToM2 = true;
    public Camera cam;


    public abstract float getLMBCooldown();

     public abstract bool isLMBCooldown();

    public abstract float getRMBCooldown();

    public abstract bool isRMBCooldown();

    protected void HitTarget(Vector3 pos, GameObject hittable)
    {
        // play fist hit event
        // FMODUnity.RuntimeManager.PlayOneShot(fistHitEvent);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        GO.transform.parent = hittable.transform;
        Destroy(GO, 20);
    }
    protected IEnumerator ResetAttackLockIn(float attackCooldown)
    {
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    protected IEnumerator AttackRaycast(float attackRange, float attackDamage, float attackDelay)
    {

        yield return new WaitForSeconds(attackDelay);
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

    abstract public void Attack_M1(CallbackContext context);
    abstract public void Attack_M2(CallbackContext context);

}