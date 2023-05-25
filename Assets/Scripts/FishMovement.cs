using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] float swimSpeed;
    public Vector3 direction;
    private Rigidbody rb;
    private bool baited;
    private Vector3 setBaitPosition;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(Swim), 1, 5);
    }
    private void FixedUpdate()
    {
        IsBaited();
    }
    void IsBaited()
    {
        if (baited)
        {


            direction = (setBaitPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
        }
    }
    // Coroutine to add force to fish's rigidbody
    void Swim()
    {
        rb.AddForce(swimSpeed * Time.fixedDeltaTime * direction, ForceMode.Impulse);
        rb.velocity = Vector3.zero;
    }
    //Move towards bait if triggered
    public void GetBaited(Vector3 baitPosition)
    {
        setBaitPosition = baitPosition;
        baited = true;
    }

}