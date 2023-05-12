using UnityEngine;

public class Interactible : MonoBehaviour
{
    // Start is called before
    // iytrirst frame update
    public GameObject interactibleIcon;
    public bool triggered;
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            Debug.Log(transform.name);
            if (transform.name == "Bed")
            {
                Sleep sleep = GetComponent<Sleep>();
                sleep.GoToSleep();
            }
            else if (transform.name == "Door")
            {
                TransitArea openDoor = GetComponent<TransitArea>();
                openDoor.Transit();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactibleIcon.SetActive(true);
        }
        else
        {
            interactibleIcon.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactibleIcon.SetActive(false);
        }
    }
}
