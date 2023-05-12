using UnityEngine;

public class Interactible : MonoBehaviour
{
    public GameObject interactibleIcon;
    public bool isActivated;



    // Update is called once per frame
    void Update()
    {
        checkActivated();
    }

    private void checkActivated()
    {
        if (isActivated)
        {
            switch (transform.name)
            {
                case "Bed":
                    Sleep sleep = GetComponent<Sleep>();
                    sleep.GoToSleep();
                    break;
                case "Door":
                    TransitArea openDoor = GetComponent<TransitArea>();
                    openDoor.Transit();
                    break;
                case "Character":
                    Conversation characterInteraction = GetComponent<Conversation>();
                    characterInteraction.StartDialog();
                    break;
            }
            isActivated = false;
        }
    }
    //Shows icon when player enters the trigger
    private void OnTriggerEnter(Collider other)
    {

        interactibleIcon.SetActive(other.CompareTag("Player"));

    }
    //Hides the icon when player exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactibleIcon.SetActive(false);
        }
    }
}
