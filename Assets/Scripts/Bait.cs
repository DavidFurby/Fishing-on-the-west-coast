using UnityEngine;

public class Bait : MonoBehaviour
{
    [SerializeField] GameObject sea;
    private Rigidbody rb;
    public bool inWater = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            other.GetComponent<FishMovement>().GetBaited(transform.position);
        }
        else if (other.gameObject == sea)
        {
            inWater = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == sea)
        {
            inWater = false;
        }
    }
}
