using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    Vector3 velocity;
    public float gravity = -9.8f;
    
    //Used to check ground
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistacne = 0.4f;
    public LayerMask groundMask;
    public float jumpHight = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Chcek if grouned
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistacne, groundMask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Character Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        //Gravity added to player
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Added Jumping motion Chage jump hight to mod
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHight * -2 * gravity);
        }
        
    }
}
