using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // Speed of the player
    [SerializeField] private Vector2 minBounds; // Minimum (x, y) world position
    [SerializeField] private Vector2 maxBounds; // Maximum (x, y) world position

    private InputSystem_Actions inputActions; // Reference to the input actions
    private Vector2 movemet;
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    private void Awake()
    {
        inputActions = new InputSystem_Actions(); // Initialize the input actions
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to this GameObject
    }

    private void OnEnable()
    {
        inputActions.Enable(); // Enable the input actions
    }

    private void Update()
    {
        PlayerInput(); // Call the method to get player input
    }

    private void FixedUpdate()
    {
        Move(); // Call the Move method
    }

    private void PlayerInput()
    {
        movemet = inputActions.Player.Move.ReadValue<Vector2>(); // Get the movement input from the player
    }

    private void Move()
    {
        // Calculate new position
        Vector2 newPosition = rb.position + movemet * (moveSpeed * Time.fixedDeltaTime);

        // Clamp the new position within the defined bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        // Apply movement to the Rigidbody2D
        rb.MovePosition(newPosition);
    }

}
