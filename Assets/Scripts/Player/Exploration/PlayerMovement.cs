using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Constants for movement and rotation speeds
    private const int MovementSpeed = 10;
    private const int RotationSpeed = 50;

    // Method to move the player in a specific direction
    internal void MovePlayer(Vector3 movementDirection)
    {
        transform.Translate(MovementSpeed * Time.fixedDeltaTime * movementDirection, Space.World);
    }

    // Method to rotate the player in the direction
    internal void RotatePlayer(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion newRotation = CalculateNewRotation(direction);
            newRotation.z = gameObject.transform.rotation.z;
            newRotation.x = gameObject.transform.rotation.x;
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, newRotation, RotationSpeed);
        }
    }

    // Method to calculate the new rotation based on the movement direction
    private Quaternion CalculateNewRotation(Vector3 movementDirection)
    {
        return Quaternion.LookRotation(movementDirection);
    }

    // Method to check if the player is grounded
    internal bool IsGrounded()
    {
        Vector3 offset = transform.rotation * (Vector3.forward * 0.5f + Vector3.up * 0.01f);
        return Physics.Raycast(transform.position + offset, -transform.up, out _, 1f);
    }
}
