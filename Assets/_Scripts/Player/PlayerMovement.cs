using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    InputManager inputManager; // Input Manager instance

    Vector3 moveDirection; // Direction player moves
    Transform cameraTransform; // Transform of the camera the player sees through
    Rigidbody playerRigidBody; // Reference to player's RigidBody component

    [Header("Movement Stats")]
    public float movementSpeed = 5f; // How fast the player can move
    public float rotationSpeed = 15f;

    // Called right before Start() method
    private void Awake()
    {
        inputManager = GetComponent<InputManager>(); // Reference to InputManager attached to player
        playerRigidBody = GetComponent<Rigidbody>(); // Reference to RigidBody attached to player
        cameraTransform = Camera.main.transform; // Get transform of the main camera
    }

    // Public method to call the other movement functions
    public void HandleAllPlayerMovement()
    {
        HandlePlayerMovement(); // Handles the movement on the x and z axes
        HandlePlayerRotation(); // Handles the rotation of the player
    }

    // Handles the movement for the player ni the x and z axes
    private void HandlePlayerMovement()
    {
        moveDirection = cameraTransform.forward * inputManager.verticalInput; // Get direction of vertical movement
        moveDirection = moveDirection + cameraTransform.right * inputManager.horizontalInput; // Get direction of horizontal movement
        moveDirection.Normalize(); // Change length of vector to 1
        moveDirection.y = 0; // Player should not move on the y axis

        Vector3 movementVelocity = moveDirection * movementSpeed; // Velocity of player movement
        playerRigidBody.velocity = movementVelocity; // Change the RigidBody attached to the player to match the movementVelocity variable
    }

    private void HandlePlayerRotation()
    {
        Vector3 targetDirection = Vector3.zero; // Start out at (0, 0, 0)
        targetDirection = cameraTransform.forward * inputManager.verticalInput; // Face player in direction of vertical movement
        targetDirection = targetDirection + cameraTransform.right * inputManager.horizontalInput; // Face player in direction of horizontal movement
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) targetDirection = transform.forward; // Keep rotation at position last specified by the player

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection); // Look towards the target direction defined above
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Get quaternion of the player's roation

        transform.rotation = playerRotation; // Rotate the transform of the player
    }
}