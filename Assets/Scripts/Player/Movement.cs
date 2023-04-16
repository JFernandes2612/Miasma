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

    private Vector3 moveInput;

    private Rigidbody rb;

    private PlayerMovement playerMovement;

    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = new PlayerMovement();
        playerMovement.Enable();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Check player movement
        moveInput = playerMovement.Player_Map.Movement.ReadValue<Vector3>();

        if (isGrounded)
        {
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
        else if (moveInput != Vector3.zero)
        {
            Vector3 wishDir = (moveInput.x * transform.right + moveInput.z * transform.forward).normalized;
            float currentSpeed = Vector3.Dot(rb.velocity, wishDir);

            float addSpeed = baseMovementSpeed - currentSpeed;

            if (addSpeed <= 0.0f)
                return;

            float accelSpeed = airAcceleration * maxAirSpeed;

            if (accelSpeed > addSpeed)
                accelSpeed = addSpeed;

            rb.velocity += accelSpeed * wishDir;

            rb.velocity = Vector3.ClampMagnitude(new Vector3(rb.velocity.x, 0.0f, rb.velocity.z), maxAirSpeed) + rb.velocity.y * Vector3.up;
        }

        // Log current absolute horizontal speed
        Debug.Log(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
    }

    void Update()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.contacts.Length > 0)
        {
            foreach (ContactPoint contact in collision.contacts)
                if (contact.normal.y > 0.0f)
                {
                    isGrounded = true;
                    return;
                }
        }

        isGrounded = false;
    }
}
