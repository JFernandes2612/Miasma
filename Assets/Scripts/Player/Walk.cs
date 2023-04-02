using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float jumpForce;

    private Vector3 moveInput;

    private Rigidbody rb;

    private PlayerMovement playerMovement;

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

        // Check jump
        playerMovement.Player_Map.Jump.performed += ctx => {
            rb.AddForce(Vector3.up * jumpForce);
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
