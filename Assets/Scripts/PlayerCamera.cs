using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, 3, -6);
    }
}
