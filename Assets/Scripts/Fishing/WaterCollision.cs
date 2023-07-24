using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    [SerializeField] private GameObject bait;
    [SerializeField] private FishingSystem system;
    [SerializeField] private AudioSource splashSound;
    private float waterLevel;


    void Start()
    {
        waterLevel = transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == bait && system != null)
        {
            if (system.GetCurrentState() is Casting)
            {
                system.baitLogic.UpdateDistanceRecord();
                system.SetState(new Fishing(system));
            }
            splashSound.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var rigidBody = other.GetComponent<Rigidbody>();
        if (rigidBody != null)
        {
            Float(rigidBody, 150f, 1f);
        }
    }

    public void Float(Rigidbody rigidBody, float floatHeight, float bounceDamp)
    {
        rigidBody.drag = 2f;
        Vector3 actionPoint = transform.position + transform.TransformDirection(Vector3.down);
        float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
        if (forceFactor > 0f)
        {
            Vector3 uplift = -Physics.gravity * (forceFactor - rigidBody.velocity.y * bounceDamp);
            rigidBody.AddForceAtPosition(uplift, actionPoint);
        }
    }
}
