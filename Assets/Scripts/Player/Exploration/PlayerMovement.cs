using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const int movementSpeed = 10;
    private const int rotationSpeed = 50;
    protected PlayerManager manager;

    public void Initialize(PlayerManager manager)
    {
        this.manager = manager;
    }
    internal virtual void MoveCharacter(Vector3 direction)
    {
        transform.Translate(movementSpeed * Time.fixedDeltaTime * direction, Space.World);
    }
    internal void RotateTowardsInteractive()
    {
        if (manager.interactive != null)
        {
            Vector3 direction = manager.interactive.transform.position - transform.position;
            manager.movement.RotateCharacter(direction);
        }
    }
    internal virtual void RotateCharacter(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion rotation = CalculateNewRotation(direction);
            rotation.z = gameObject.transform.rotation.z;
            rotation.x = gameObject.transform.rotation.x;
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, rotation, rotationSpeed);
        }
    }
    // Method to calculate the new rotation based on the movement direction
    private Quaternion CalculateNewRotation(Vector3 direction)
    {
        return Quaternion.LookRotation(direction);
    }

    // Method to check if the player is grounded
    internal virtual bool IsGrounded()
    {
        Vector3 offset = transform.rotation * (Vector3.forward * 0.5f + Vector3.up * 0.01f);
        return Physics.Raycast(transform.position + offset, -transform.up, out _, 1f);
    }
}
