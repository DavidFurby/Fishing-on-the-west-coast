using UnityEngine;

public class Bait : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            Debug.Log("Baited");
            other.GetComponent<FishMovement>().GetBaited(transform.position);
        }
    }
    public void SetBait()
    {

    }
}
