using UnityEngine;

public class Interactible : MonoBehaviour
{
    [SerializeField] GameObject interactibleIcon;

    public void CheckActivated()
    {
        Component[] components = GetComponents(typeof(Component));
        foreach (Component component in components)
        {
            if (component is IInteractible)
            {
                (component as IInteractible).Interact();
                break;
            }
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
