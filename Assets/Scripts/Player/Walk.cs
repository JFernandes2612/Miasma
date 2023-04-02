using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

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
        moveInput = playerMovement.Player_Map.Movement.ReadValue<Vector3>();
        Vector3 inputSpeed = moveInput.normalized * movementSpeed;
        rb.velocity = new Vector3(inputSpeed.x, rb.velocity.y, inputSpeed.z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
