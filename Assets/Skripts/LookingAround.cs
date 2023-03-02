using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LookingAround : NetworkBehaviour
{
    public float Msensitivity = 500f;
    public Transform lebody;
    [SerializeField] private Transform playerCameraMove;

    public static bool locklook = false;
    
    float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if(locklook == false)
        {
            float mouseX = Input.GetAxis("Mouse X") * Msensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * Msensitivity * Time.deltaTime;

            lebody.Rotate(Vector3.up * mouseX);

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCameraMove.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
