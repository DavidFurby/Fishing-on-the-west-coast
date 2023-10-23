
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private const int MovementSpeed = 10;
    private readonly float interpolationSpeed = 10;


    internal void MovePlayer(Vector3 movementDirection)
    {
        transform.Translate(MovementSpeed * Time.fixedDeltaTime * movementDirection, Space.World);
    }

    internal void RotatePlayer(Vector3 movementDirection)
    {
        Quaternion newRotation;
        if (movementDirection != Vector3.zero)
        {
            newRotation = Quaternion.LookRotation(movementDirection);
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, newRotation, 50);

        }
    }

    internal void GroundAlignment()
    {
        if (IsGrounded())
        {
            Vector3 rayOrigin = transform.position - new Vector3(0, transform.localScale.y / 2, 0);
            if (Physics.Raycast(rayOrigin, -transform.up, out RaycastHit hit, 1))
            {
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * transform.rotation, interpolationSpeed * Time.deltaTime);
            }
        }
    }
    internal bool IsGrounded()
    {
        Vector3 offset = transform.rotation * (Vector3.forward * 0.5f + Vector3.up * 0.01f);

        return Physics.Raycast(transform.position + offset, -transform.up, out _, 1f);
    }
}