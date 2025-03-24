using UnityEngine;

/// <summary>
/// This class is responsible for the movement of the player in the Capture The Flag game.
/// It is able to move an object when provided a movement vector.
///
/// Will be used by the MLAgent to easily move the player.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CaptureTheFlagMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    
    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float groundCheckRadius = 0.2f;
    
    /// <summary>
    /// Is the object currently grounded?
    /// </summary>
    public bool IsGrounded => 
        Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayers);

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Moves the object based on the provided movement vector.
    /// </summary>
    /// <param name="movementVector">The movement vector to move the object.</param>
    public void Move(Vector3 movementVector)
    {
        rb.velocity = movementVector * movementSpeed;
    }

    /// <summary>
    /// Applies an upward force to the object to simulate a jump.
    /// </summary>
    public void Jump()
    {
        if (!IsGrounded) return;
        
        rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
    }


    private void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
