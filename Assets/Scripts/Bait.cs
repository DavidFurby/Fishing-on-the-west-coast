using UnityEngine;

public class Bait : MonoBehaviour
{
    [SerializeField] Rope rope;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            other.GetComponent<FishMovement>().GetBaited(transform.position);
        }
    }
}
