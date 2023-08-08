using UnityEngine;

public class Interactive : MonoBehaviour
{
    [SerializeField] GameObject InteractiveIcon;

    public void CheckActivated()
    {
        Component[] components = GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component is IInteractive)
            {
                (component as IInteractive).Interact();
                break;
            }
        }
    }

    //Shows icon when player enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        InteractiveIcon.SetActive(other.CompareTag("Player"));
    }
    //Hides the icon when player exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractiveIcon.SetActive(false);
        }
    }
}
