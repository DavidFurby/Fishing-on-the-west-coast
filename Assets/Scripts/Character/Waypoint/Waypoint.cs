using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public GameCharacters character;
    //What position in the order of waypoints for the character this represents
    public int orderPosition;

    void OnDrawGizmos()
    {
        // Draw a small sphere in the Scene view at the position of the waypoint for easy visualization
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
