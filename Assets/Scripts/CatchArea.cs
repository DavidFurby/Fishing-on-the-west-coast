using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public bool isInCatchArea;
    private GameObject fish;
    [SerializeField] GameObject bait;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            isInCatchArea = true;
            fish = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            isInCatchArea = false;
            fish = null;
        }
    }
    public void CatchFish()
    {
        Debug.Log("Catch Fish");
        Debug.Log(fish);
        Rigidbody fishRigidBody = fish.GetComponent<Rigidbody>();
        fishRigidBody.isKinematic = true;
        fish.transform.position = transform.position;
    }
}
