using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
 
    public float walkSpeed;
    public float sprintSpeed;
    public float gravity;
    public float jumpHeight;
 
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
 
    Vector3 velocity;
 
    bool isGrounded;
    void Start () {

        walkSpeed = 3f;
        sprintSpeed = 6f;
        jumpHeight = 1f;
        gravity = -9.81f * 2;

    }

    // Update is called once per frame
    void Update()
    {
        //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
 
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (InventorySystem.Instance.isOpen == true)
        {
            x = 0;
            z = 0;
        }
        //right is the red Axis, foward is the blue axis
        Vector3 move = transform.right * x + transform.forward * z;
 
        controller.Move(move * walkSpeed * Time.deltaTime);
 
        if(InventorySystem.Instance.isOpen == false)
        {
            //check if the player is on the ground so he can jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                //the equation for jumping
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        //make player sprint when holding shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //increase speed for sprinting
            walkSpeed = sprintSpeed;
        }
        else
        {
            walkSpeed = 3f;
        }
 
        velocity.y += gravity * Time.deltaTime;
 
        controller.Move(velocity * Time.deltaTime);
    }
}
