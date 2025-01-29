using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = 9.81f;
    public float jumpHeight = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
         controller = GetComponent<CharacterController>();
    
        if (controller == null)
        {
            Debug.LogError("CharacterController component is missing on " + gameObject.name);
            enabled = false; // Disable script if the component is missing
        }
    }
    
    void OnTriggerEnter(Collider other)
{
    if (other.gameObject.CompareTag("tooth"))
    {
        // Collect the teeth
        Debug.Log("Tooth Collected!");

        // Destroy the teeth object
        Destroy(other.gameObject);
    }
}
    void Update()
    {
        // Check if the character is on the ground
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small value to keep grounded
        }

        // Get input for movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        // Apply gravity
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
