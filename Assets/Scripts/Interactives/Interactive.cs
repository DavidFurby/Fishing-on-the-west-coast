using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactive : MonoBehaviour
{
    private GameObject iconInstance;
    private const string ItemsPath = "GameObjects/Interactive/";
    public static event Action<GameObject> OnEnterInteractive;
    public static event Action OnExitInteractive;


    private void Start()
    {
        InstantiateInteractiveIcon();
    }

    private void InstantiateInteractiveIcon()
    {
        var InteractiveIcon = Resources.Load<GameObject>(ItemsPath + "InteractiveIcon");
        if (InteractiveIcon != null)
        {
            iconInstance = Instantiate(InteractiveIcon, transform.position + Vector3.up * 2f, Quaternion.identity);
            iconInstance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            iconInstance.transform.SetParent(transform);
            iconInstance.AddComponent<IconAnimator>();
            iconInstance.SetActive(false);
        }
    }



    public void StartInteraction()
    {
        if (TryGetComponent<IInteractive>(out var interactiveComponent) && iconInstance.activeSelf)
        {
            interactiveComponent.Interact();
            HideIcon();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>().interactive == null)
        {
            ShowIcon();
            OnEnterInteractive.Invoke(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>().interactive == this)
        {
            HideIcon();
            OnExitInteractive.Invoke();
        }
    }

    private void ShowIcon()
    {
        if (iconInstance != null)
        {
            iconInstance.SetActive(true);
        }
    }

    private void HideIcon()
    {
        if (iconInstance != null)
        {
            iconInstance.SetActive(false);
        }
    }
}
