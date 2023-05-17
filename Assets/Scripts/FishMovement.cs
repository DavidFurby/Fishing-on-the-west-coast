using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float swimSpeed;
    public Vector3 direction;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(Swim), 1, 4);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    // Method to move fish based on speed and direction
    void FloatSpeed()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }
    // Coroutine to add force to fish's rigidbody
    void Swim()
    {
        rb.AddForce(direction * swimSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
    }
    //Move towards bait if triggered
    public void GetBaited(Vector3 baitPosition)
    {
        direction = (baitPosition - transform.position).normalized;
    }

}
