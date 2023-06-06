using UnityEngine;

public class MusicButtonController : MonoBehaviour
{
    [SerializeField] KeyCode keyCode;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (Input.GetKeyDown(keyCode))
        {
            Debug.Log(keyCode);
            Destroy(other.gameObject);
        }
    }
}