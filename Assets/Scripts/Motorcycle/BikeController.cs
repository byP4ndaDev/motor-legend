using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BikeController : MonoBehaviour
{
    // Variables
    [SerializeField] private WheelJoint2D frontTire, backTire;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rotationSpeed;

    private float movement, moveSpeed;

    public Vector3 StartPos { get; set; }
    
    public CustomTrigger DeathTrigger;
    public CustomTrigger GroundTrigger;
    public GameManager gameManager;

    private bool isDead = false;
    private bool isGrounded = false;

    private void Awake()
    {
        // Add the callable Functions to the Triggers
        DeathTrigger.EnteredTrigger += OnDeathTriggerEntered;
        GroundTrigger.EnteredTrigger += OnGroundTriggerEntered;
        GroundTrigger.ExitedTrigger += OnGroundTriggerExited;
    }

    private void Update()
    {
        // Check the keyboard input
        movement = -Input.GetAxis("Horizontal");
        moveSpeed = movement * speed;
    }

    private void FixedUpdate() 
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, StartPos.x, transform.position.x), transform.position.y);
        
        // Check if player grounded
        if (isGrounded)
        {
            // If there is no movement, deactivate the motor
            if(moveSpeed.Equals(0)) { 
                frontTire.useMotor = false;
                backTire.useMotor = false;
            }

            // If there is movement, activate the motor and set the motorspeed;
            else 
            {
                frontTire.useMotor = true;
                backTire.useMotor = true;
                JointMotor2D motor = new JointMotor2D();
                motor.motorSpeed = moveSpeed;
                motor.maxMotorTorque = 10000;
                frontTire.motor = motor;
                backTire.motor = motor;
            }
        }
        else
        {
            // If not grounded add torque
            if (!movement.Equals(0))
            {
                rb.AddTorque(-movement * rotationSpeed);
            }
        }
        // if the player is dead, set the velocity to 0
        if(isDead) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }


    public void OnDeath()
    {
        // If the player is dead, end the game
        if (!isDead)
        {
            DeathTrigger.gameObject.SetActive(false);
            isDead = true;
            gameManager.EndGame();
        }
    }

    void OnDeathTriggerEntered(Collider2D other)
    {
        OnDeath();
    }
    void OnGroundTriggerEntered(Collider2D other)
    {
        Debug.Log("Ground");
        isGrounded = true;
    }
    void OnGroundTriggerExited(Collider2D other)
    {
        Debug.Log("Not Ground");
        isGrounded = false;
    }
}
