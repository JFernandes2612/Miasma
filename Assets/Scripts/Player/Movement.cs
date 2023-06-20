using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float baseMovementSpeed;

    [SerializeField]
    private float backMovementSpeed;

    [SerializeField]
    private float maxAirSpeed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float acceleration;

    [SerializeField]
    private float deceleration;

    [SerializeField]
    private float airAcceleration;

    [SerializeField]
    private float coyoteTime;
    private float coyoteTimeCounter;

    [SerializeField]
    private float jumpBufferTime = 0.2f;

    private float jumpBufferCounter;


    private Vector3 moveInput;

    private Rigidbody rb;

    [SerializeField]
    private Vector3 boxsize = new Vector3(0.5f, 0.5f, 0.5f);

    private PlayerInput playerMovement;

    private float distanceToGround;

    private bool canQuickStep = true;
    public bool isAnimLocked = false;

    [SerializeField]
    private float dashingPower = 24f;


    [SerializeField]
    private float dashTime = 0.3f;
    [SerializeField]
    private float dashCooldown = 1f;

    private Player player;

    private float slowAmount = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = new PlayerInput();
        playerMovement.Enable();
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void SlowPlayer()
    {
        baseMovementSpeed = baseMovementSpeed * slowAmount;
    }

    public void UnSlowPlayer()
    {
        baseMovementSpeed = baseMovementSpeed / slowAmount;
    }


    void FixedUpdate()
    {

        if (isAnimLocked) return;

        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTime;

            if (canQuickStep && playerMovement.Player_Map.QuickStep.IsPressed())
            {
                StartCoroutine(QuickStep());
                coyoteTimeCounter = 0f;
                return;
            }

            GroundMove();
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;

            if (moveInput != Vector3.zero)
            {
                AirAccel();
            }
        }


        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            coyoteTimeCounter = 0f;
            //reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpBufferCounter = 0f;
        }

    }

    void Update()
    {
        moveInput = playerMovement.Player_Map.Movement.ReadValue<Vector3>();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private IEnumerator QuickStep()
    {
        isAnimLocked = true;
        canQuickStep = false;
        Vector3 wishSpeed = (moveInput.x * transform.right + moveInput.z * transform.forward).normalized;
        rb.velocity = wishSpeed * dashingPower;
        player.setInvincible(true);
        yield return new WaitForSeconds(dashTime);
        rb.velocity = Vector3.zero;
        isAnimLocked = false;
        player.setInvincible(false);
        yield return new WaitForSeconds(dashCooldown);
        canQuickStep = true;
    }


    bool isGrounded()
    {
        //return Physics.BoxCast(transform.position, boxsize, -transform.up, transform.rotation, distanceToGround + 0.1f);
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);
    }

    private void AirAccel()
    {
        Vector3 wishDir = (moveInput.x * transform.right + moveInput.z * transform.forward).normalized;
        float currentSpeed = Vector3.Dot(rb.velocity, wishDir);

        float addSpeed = baseMovementSpeed - currentSpeed;

        if (addSpeed <= 0.0f)
            return;

        rb.velocity += addSpeed * airAcceleration * wishDir;

        rb.velocity = Vector3.ClampMagnitude(new Vector3(rb.velocity.x, 0.0f, rb.velocity.z), maxAirSpeed) + rb.velocity.y * Vector3.up;
    }

    private void GroundMove()
    {
        float dynamicSpeedFactor = 1;

        if (moveInput != Vector3.zero)
        {
            //move
            Vector3 wishDir = (moveInput.x * transform.right + moveInput.z * transform.forward).normalized;
            dynamicSpeedFactor = (Vector3.Dot(wishDir, transform.forward) + 1.0f) / 2.0f;
            rb.velocity += wishDir * acceleration * (1 + dynamicSpeedFactor);
        }
        else if (rb.velocity != Vector3.zero)
        {
            //decelerate
            rb.velocity -= rb.velocity * deceleration;
        }

        float maxSpeed = Mathf.Lerp(backMovementSpeed, baseMovementSpeed, dynamicSpeedFactor);

        rb.velocity = Vector3.ClampMagnitude(new Vector3(rb.velocity.x, 0.0f, rb.velocity.z), maxSpeed) + rb.velocity.y * Vector3.up;
    }
}
