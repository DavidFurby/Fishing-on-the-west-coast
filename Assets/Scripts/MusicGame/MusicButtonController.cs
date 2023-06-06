using UnityEngine;

public class MusicButtonController : MonoBehaviour
{
    [SerializeField] KeyCode keyCode;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(keyCode))
        {
            Debug.Log(keyCode);
            Destroy(other.gameObject);
        }
    }
}