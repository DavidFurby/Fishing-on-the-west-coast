using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactive : MonoBehaviour
{
    private GameObject iconInstance;
    private const string ItemsPath = "GameObjects/Interactive/";
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
        if (TryGetComponent<IInteractive>(out var interactiveComponent))
        {
            interactiveComponent.Interact();
            HideIcon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowIcon();
        }
    }

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
