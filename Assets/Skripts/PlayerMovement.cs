using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class PlayerMovement : NetworkBehaviour
{
    CharacterController controller;

    public float speed = 10f;
    public float walkSpeed = 8;
    public float runSpeed = 15f;

    public float jumpheight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    Vector3 velocity;

    [SerializeField] private GameObject playerCamera;

    public float gravity = -19.62f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Debug.Log("player movement started wooow yoay oawolk");
        if (!IsOwner) return;
        playerCamera.SetActive(true);
    }


    void Update()
    {
        if (!IsOwner) return;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded == true && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            speed = runSpeed;
        }
        else{
            speed = walkSpeed;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
        }
        controller.Move(velocity * Time.deltaTime);

    }


}