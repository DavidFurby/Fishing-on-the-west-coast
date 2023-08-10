using UnityEngine;

public class Interactive : MonoBehaviour
{
    [SerializeField] private GameObject InteractiveIcon;

    public void CheckActivated()
    {
        Component[] components = GetComponents<Component>();
        foreach (Component component in components)
        {
        Debug.Log("interact");
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
        if (other.CompareTag("Player"))
        {
            ShowIcon();
        }
    }

    //Hides the icon when player exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideIcon();
        }
    }

    public void ShowIcon()
    {
        if (InteractiveIcon != null)
        {
            InteractiveIcon.SetActive(true);
        }
    }

    public void HideIcon()
    {
        if (InteractiveIcon != null)
        {
            InteractiveIcon.SetActive(false);
        }
    }
}
