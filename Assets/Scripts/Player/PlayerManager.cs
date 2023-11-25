using UnityEngine;
using System;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerSubscriptions))]
[RequireComponent(typeof(PlayerAnimations))]
[RequireComponent(typeof(Interactive))]
[RequireComponent(typeof(FishingController))]
public class PlayerManager : PlayerEventController
{
    public static PlayerManager Instance { get; private set; }
    internal PlayerAnimations animations;
    internal Interactive interactive;
    internal PlayerMovement movement;
    internal PlayerSubscriptions subscriptions;
    internal FishingController fishingController;

    public static event Action OnNavigateShop;
    public static event Action OnOpenItemMenu;

    void Awake()
    {
        InitializeSingleton();
        InitializeComponents();

    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = new Vector3(0, -0.5f, 0);
    }


    private void InitializeComponents()
    {
        animations = GetComponentInChildren<PlayerAnimations>();
        movement = GetComponentInChildren<PlayerMovement>();
        subscriptions = GetComponent<PlayerSubscriptions>();
        fishingController = GetComponent<FishingController>();
        movement.Initialize(Instance);
        subscriptions.Initialize(Instance);
        fishingController.Initialize(Instance);
        animations.Initialize(Instance);
        SetState(new ExplorationIdle());
    }

    public void HandleInput()
    {
        ProcessMovementInput();
        ProcessInteractionInput();
    }

    private void ProcessMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        animations.SetWalkAnimation(horizontalInput != 0 || verticalInput != 0);
    }

    private void ProcessInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && interactive != null)
        {
            animations.SetWalkAnimation(false);
            ActivateInteractive();
        }
    }

    public void HandleMovement()
    {
        Vector3 direction = new(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        if (movement.IsGrounded())
        {
            movement.MoveCharacter(direction);

        }
        movement.RotateCharacter(direction);
    }
 

    internal void SetInteractive(GameObject interactiveObject)
    {
        interactive = interactiveObject.GetComponent<Interactive>();
    }
    internal void RemoveInteractive()
    {
        interactive = null;
    }

    public void RaiseOpenItemMenuEvent() => OnOpenItemMenu?.Invoke();
    public void RaiseNavigateShopEvent() => OnNavigateShop?.Invoke();

    //Return to idle state only if current state is dialog
    public void ReturnControls()
    {
        if (GetCurrentState() is PlayerInDialog)
        {
            SetState(new ExplorationIdle());
            CameraManager.Instance.EnterPlayerState();
        }
    }

    private void ActivateInteractive()
    {
        SetState(new Interacting());
        interactive.StartInteraction();
    }
    internal void SetPlayerPositionAndRotation(Vector3 position, Quaternion quaternion)
    {
        transform.SetPositionAndRotation(position, quaternion);
    }
}

