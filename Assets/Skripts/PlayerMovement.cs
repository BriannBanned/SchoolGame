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
    public bool isImobile = true;

    Vector3 velocity;

    [SerializeField] private GameObject playerCamera;
    [SerializeField] private PlayerStatsScript playerStatsScript;

    public float gravity = -19.62f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Debug.Log("player movement started wooow yoay oawolk");
        transform.position = new Vector3(0, 1000, 0);
        if (!IsOwner) return;
        playerCamera.SetActive(true);

    }

    public void gotoPosition(Vector3 newPos)
    {
        print(newPos);
        transform.position = newPos;
    }

    void Update()
    {
        switch (gameObject.GetComponent<PlayerStatsScript>().playerClass.Value)
        {
            case 1: //light
                walkSpeed = 12.5f;
                runSpeed = 15.5f;
                jumpheight = 3.2f;
                playerStatsScript.maxPlayerHealth = 75;
                break;
            case 2: //balanced
                walkSpeed = 10f;
                runSpeed = 14f;
                playerStatsScript.maxPlayerHealth = 100; //could mess with items that affect health (might wanna get this out of update or smth.)
                break;
            case 3: //heavy
                walkSpeed = 7f;
                runSpeed = 12.5f;
                playerStatsScript.maxPlayerHealth = 200;
                break;
        }
        if (isImobile)
        {
            return;
        }
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
        if(playerStatsScript.isPlayerDead.Value){ //i cant just return out of this for some reason because the character goes flying idk?!?!?!
            speed = 0f;
            velocity.y = 0f;
        }

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