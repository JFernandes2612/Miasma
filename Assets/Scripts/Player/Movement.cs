using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float baseMovementSpeed;

    [SerializeField]
    private float maxAirSpeed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float acceleration;

    [SerializeField]
    private float airAcceleration;

    [SerializeField]
    private float coyoteTime;

    private float jumpBufferCounter;

    [SerializeField]
    private float jumpBufferTime = 0.2f;
    private float coyoteTimeCounter;
    private Vector3 moveInput;

    private Rigidbody rb;

    private PlayerInput playerMovement;

    private float distanceToGround;

    private bool canDash = true;
    private bool isDashing = false;

    [SerializeField]
    private float dashingPower = 24f;


    [SerializeField]
    private float dashTime = 0.3f;
    [SerializeField]
    private float dashCooldown = 1f;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = new PlayerInput();
        playerMovement.Enable();
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
    }

    void FixedUpdate()
    {
        // Check player movement
        if (isDashing) return;
        moveInput = playerMovement.Player_Map.Movement.ReadValue<Vector3>();

        if (isGrounded())
        {

            coyoteTimeCounter = coyoteTime;
            if (moveInput != Vector3.zero)
            {
                Vector3 inputSpeed = moveInput.normalized * baseMovementSpeed * acceleration;
                rb.velocity += (inputSpeed.x * transform.right + inputSpeed.z * transform.forward);
            }
            else if (rb.velocity != Vector3.zero)
            {
                rb.velocity -= rb.velocity * acceleration;
            }

            rb.velocity = Vector3.ClampMagnitude(new Vector3(rb.velocity.x, 0.0f, rb.velocity.z), baseMovementSpeed) + rb.velocity.y * Vector3.up;

        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }


        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            coyoteTimeCounter = 0f;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpBufferCounter = 0f;
        }
        if (moveInput != Vector3.zero)
        {

            Vector3 wishDir = (moveInput.x * transform.right + moveInput.z * transform.forward).normalized;
            float currentSpeed = Vector3.Dot(rb.velocity, wishDir);

            float addSpeed = baseMovementSpeed - currentSpeed;

            if (addSpeed <= 0.0f)
                return;

            rb.velocity += addSpeed * airAcceleration * wishDir;

            rb.velocity = Vector3.ClampMagnitude(new Vector3(rb.velocity.x, 0.0f, rb.velocity.z), maxAirSpeed) + rb.velocity.y * Vector3.up;
        }

        if (playerMovement.Player_Map.QuickStep.IsPressed() && canDash)
        {
            StartCoroutine(Dash());
        }

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        //rb.AddForce(moveInput * dashingPower, ForceMode.Impulse);
        rb.AddForce(transform.forward * dashingPower, ForceMode.Impulse);
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);
    }
}
