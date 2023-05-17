using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] float swimSpeed;
    public Vector3 direction;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(Swim), 1, 4);
    }
    // Coroutine to add force to fish's rigidbody
    void Swim()
    {
        rb.AddForce(swimSpeed * Time.fixedDeltaTime * direction, ForceMode.Impulse);
    }
    //Move towards bait if triggered
    public void GetBaited(Vector3 baitPosition)
    {
        Debug.Log(baitPosition);
        direction = (baitPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

}
