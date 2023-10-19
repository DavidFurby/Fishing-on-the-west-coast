
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private const int MovementSpeed = 10;



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


}