using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity;

    private float xRotation = 0.0f;

    private Transform playerTransform;

    private Rigidbody playerRb;

    [SerializeField]
    private float baseFOV;

    [SerializeField]
    private float maxFOV;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        playerRb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
        playerTransform.Rotate(Vector3.up * mouseX);

        GetComponent<Camera>().fieldOfView = Mathf.Min(baseFOV + new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z).magnitude / 2.0f, maxFOV);
    }
}
