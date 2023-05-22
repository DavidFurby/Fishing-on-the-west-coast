using UnityEngine;

public class Bait : MonoBehaviour
{
    public float throwPower;
    private Rigidbody rb;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            other.GetComponent<FishMovement>().GetBaited(transform.position);
        }
    }
    public void ThrowBait()
    {
        rb.AddForce(new Vector3(1, 1, 0) * throwPower * Time.deltaTime);
    }
}
