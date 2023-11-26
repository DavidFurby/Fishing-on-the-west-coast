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
            iconInstance = InstantiateIcon(InteractiveIcon);
        }
    }

    private GameObject InstantiateIcon(GameObject InteractiveIcon)
    {
        var icon = Instantiate(InteractiveIcon, transform.position + Vector3.up * 2f, Quaternion.identity);
        icon.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        icon.transform.SetParent(transform);
        icon.AddComponent<IconAnimator>();
        icon.SetActive(false);
        return icon;
    }

    public void StartInteraction()
    {
        if (TryGetComponent<IInteractive>(out var interactiveComponent) && iconInstance.activeSelf)
        {
            interactiveComponent.Interact();
            SetIconActiveState(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayer(other) && other.GetComponent<PlayerManager>().interactive == null)
        {
            SetIconActiveState(true);
            OnEnterInteractive.Invoke(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other) && IsPlayerInteractive(other) && IsNotUsingInteractive(other))
        {
            SetIconActiveState(false);
            OnExitInteractive.Invoke();
        }
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player");
    }

    private bool IsPlayerInteractive(Collider other)
    {
        return other.GetComponent<PlayerManager>().interactive == this;
    }

    private bool IsNotUsingInteractive(Collider other)
    {
        return other.GetComponent<PlayerManager>().GetCurrentState() is not PlayerInDialog or Interacting; 
    }

    private void SetIconActiveState(bool state)
    {
        if (iconInstance != null)
        {
            iconInstance.SetActive(state);
        }
    }
}
