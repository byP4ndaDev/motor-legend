using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeControllerBackup : MonoBehaviour
{
    [Header("Vehicle")]
    [SerializeField] private Rigidbody2D vehicle;
    [SerializeField] private WheelJoint2D frontWheel;
    [SerializeField] private WheelJoint2D backWheel;

    [Header("Speed")]
    public float maxSpeed = 1500f;
    public float acceleration = 500f;
    public float deceleration = 500f;
    
    [Header("Rotation")]
    public float rotationSpeed = 500f;
    
    [Header("Points System")]
    [SerializeField] private PointsSystem pointsSystem;

    private float movement = 0f;
    private float input = 0f;
    public bool isGrounded = false;
    
    private float lastRotation = 0f;

    void Update()
    {
        input = -Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        
        if (!isGrounded) 
        {
            backWheel.useMotor = false;
            frontWheel.useMotor = false;
            vehicle.AddTorque(-input * maxSpeed * rotationSpeed * Time.fixedDeltaTime);
            if (lastRotation + 350 < vehicle.rotation)
            {
                // Front flip
                pointsSystem.AddPoints(20);
                lastRotation = vehicle.rotation;
            } 
            else if (lastRotation - 350 > vehicle.rotation)
            {
                // Back flip
                pointsSystem.AddPoints(20);
                lastRotation = vehicle.rotation;
            }
        }
        else
        {
            lastRotation = vehicle.rotation;
        }
        
        if (input != 0)
        {
            movement += input * acceleration * Time.deltaTime;
            movement = Mathf.Clamp(movement, -maxSpeed, maxSpeed);
            
            backWheel.useMotor = true;
            frontWheel.useMotor = true;

            JointMotor2D motor = new JointMotor2D { motorSpeed = movement, maxMotorTorque = 10000 };
            backWheel.motor = motor;
            frontWheel.motor = motor;
        }
        else
        {
            if (movement > 0)
                movement = Mathf.Max(0, movement - deceleration * Time.deltaTime);
            else if (movement < 0)
                movement = Mathf.Min(0, movement + deceleration * Time.deltaTime);
            
            JointMotor2D motor = new JointMotor2D { motorSpeed = movement, maxMotorTorque = 10000 };
            backWheel.motor = motor;
            frontWheel.motor = motor;
            
            backWheel.useMotor = false;
            frontWheel.useMotor = false;
        }
    }

    /*private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }*/
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
