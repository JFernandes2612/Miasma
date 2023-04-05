using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float jumpForce;

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
        Vector3 inputSpeed = moveInput.normalized * movementSpeed;
        rb.velocity = inputSpeed.x * transform.right + inputSpeed.z * transform.forward + rb.velocity.y * transform.up;
    }

    void Update()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = false;
            rb.AddForce(Vector3.up * jumpForce);
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
    }
}
