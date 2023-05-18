using UnityEngine;

public class Bait : MonoBehaviour
{
    private Rigidbody rb;
    public float throwPower;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            Debug.Log("Baited");
            other.GetComponent<FishMovement>().GetBaited(transform.position);
        }
    }
    public void ThrowBait()
    {
        Debug.Log(throwPower);
        rb.AddForce(throwPower * Time.deltaTime * new Vector3(1, 1, 0), ForceMode.Impulse);
        Debug.Log("throw");
    }
}
