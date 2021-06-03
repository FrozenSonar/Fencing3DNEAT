using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    
    
    public abstract class Command
    {
        public abstract void Execute();
    }

    public class JumpFunction : Command
    {
        public override void Execute()
        {
            Jump();
        }
    }

    public class TelekinesisFunction : Command
    {
        public override void Execute()
        {
            Telekinesis();
        }
    }


    public static void Telekinesis()
    {
        
    }

    public static void Jump()
    {
        
    }

    
    public static void DoMove()
    {
        Command keySpace = new JumpFunction();
        Command keyX = new TelekinesisFunction();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            keySpace.Execute();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            keyX.Execute();
        }


    }
    
    
    public CharacterController characterController;
    public float speed;
    
    
    public Animator animator;
    
    // camera and rotation
    public Transform cameraHolder;
    public float mouseSensitivity = 2f;
    public float upLimit = -50;
    public float downLimit = 50;
    
    // gravity
    private float gravity = 9.87f;
    private float verticalSpeed = 0;
    const float acceleration = 0.0067f;

    void Update()
    {
        Move();
        Rotate();
        Stab();
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Rotate()
    {
        float horizontalRotation = Input.GetAxis("Mouse X");
        float verticalRotation = Input.GetAxis("Mouse Y");
        
        transform.Rotate(0, horizontalRotation * mouseSensitivity, 0);
        cameraHolder.Rotate(-verticalRotation*mouseSensitivity,0,0);

        Vector3 currentRotation = cameraHolder.localEulerAngles;
        if (currentRotation.x > 180) currentRotation.x -= 360;
        currentRotation.x = Mathf.Clamp(currentRotation.x, upLimit, downLimit);
        cameraHolder.localRotation = Quaternion.Euler(currentRotation);
    }

    public void Stab()
    {

         
    }


    private void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        if (characterController.isGrounded) {
            verticalSpeed = 0;
            }
        else {
            verticalSpeed -= gravity * Time.deltaTime;
            }

        if (Input.GetKey(KeyCode.W)) {
            speed += acceleration;
        }
        else {
            speed -= acceleration;
        }

        if (Input.GetKey(KeyCode.X))
        {
            animator.SetBool("isStabbing", true);
        }


        speed = Mathf.Clamp(speed, 0, 1);
        Vector3 gravityMove = new Vector3(0, verticalSpeed, 0);
        
        Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove;
        characterController.Move(speed * Time.deltaTime * move + gravityMove * Time.deltaTime);
        
        animator.SetFloat("Speed", speed);
        animator.SetBool("isWalking", verticalMove != 0 || horizontalMove != 0);

        
        
        
    }
}