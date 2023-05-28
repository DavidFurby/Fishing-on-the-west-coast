using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 5, -4);
    }
}
