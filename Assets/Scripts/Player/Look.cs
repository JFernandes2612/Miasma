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

    private bool isLocked = false;

    [SerializeField]
    private float FOVVelocityRatio = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        playerRb = player.GetComponent<Rigidbody>();
    }

    public void LockCamera()
    {
        isLocked = true;
    }

    public void UnlockCamera()
    {
        isLocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
        playerTransform.Rotate(Vector3.up * mouseX);

        GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(GetComponent<Camera>().fieldOfView, baseFOV + Mathf.Pow(new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z).magnitude, 1.3f) * FOVVelocityRatio, Time.deltaTime * 100.0f);
    }
}
