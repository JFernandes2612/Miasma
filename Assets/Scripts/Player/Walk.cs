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
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        moveInput = playerMovement.Player_Map.Movement.ReadValue<Vector3>();
        rb.velocity = moveInput.normalized * movementSpeed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
