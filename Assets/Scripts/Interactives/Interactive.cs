using UnityEngine;

public class Interactive : MonoBehaviour
{
    private GameObject InteractiveIcon;
    private GameObject iconInstance;
   [HideInInspector] public float iconOffset = 2f;
    private const string ItemsPath = "GameObjects/";

    private void Start()
    {
        InteractiveIcon = Resources.Load<GameObject>(ItemsPath + "InteractiveIcon");

        if (InteractiveIcon != null)
        {
            iconInstance = Instantiate(InteractiveIcon, transform.position + Vector3.up * iconOffset, Quaternion.identity);
            iconInstance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            iconInstance.transform.SetParent(transform);
            iconInstance.AddComponent<IconAnimator>();
            iconInstance.SetActive(false);
        }
    }

    public void CheckActivated()
    {

        Component[] components = GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component is IInteractive)
            {
                HideIcon();
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
        if (iconInstance != null)
        {
            iconInstance.SetActive(true);
        }
    }

    public void HideIcon()
    {
        if (iconInstance != null)
        {
            iconInstance.SetActive(false);
        }
    }
}
