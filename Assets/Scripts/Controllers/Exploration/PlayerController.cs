using UnityEngine;
using System;
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : FishingController
{
    public static PlayerController Instance { get; private set; }
    private PlayerAnimations playerAnimations;
    private Interactive interactive;
    private PlayerMovement playerMovement;
    public static event Action NavigateShop;
    public static event Action OpenItemMenu;

    void Awake()
    {
        InitializeSingleton();
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

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    void Start()
    {
        InitializeComponents();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = new Vector3(0, -0.5f, 0);

    }


    private void InitializeComponents()
    {
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
        playerMovement = GetComponentInChildren<PlayerMovement>();
        SetState(new ExplorationIdle());
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        WaterCollision.OnBaitEnterSea += EnterSea;
        FishingSpot.StartFishing += (_, _) => SetState(new FishingIdle());
        FishingSpot.StartFishing += SetPlayerPositionAndRotation;
        OnEnterReelingFish += CatchFish;
        OnEnterReelingFish += () => SetState(new ReelingFish());
        OnEnterIdle += ResetValues;
        OnWhileCharging += ChargeCasting;
        OnWhileCharging += Release;
        OnWhileFishing += StartReeling;
        DialogManager.OnEndDialog += ReturnControls;
    }

    private void UnsubscribeFromEvents()
    {
        WaterCollision.OnBaitEnterSea -= EnterSea;
        FishingSpot.StartFishing -= (_, _) => SetState(new FishingIdle());
        FishingSpot.StartFishing -= SetPlayerPositionAndRotation;
        OnEnterReelingFish -= CatchFish;
        OnEnterReelingFish -= () => SetState(new ReelingFish());
        OnEnterIdle -= ResetValues;
        OnWhileCharging -= ChargeCasting;
        OnWhileCharging -= Release;
        OnWhileFishing -= StartReeling;
        DialogManager.OnEndDialog -= ReturnControls;
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

        playerAnimations.SetPlayerWalkAnimation(horizontalInput != 0 || verticalInput != 0);
    }

    private void ProcessInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && interactive != null)
        {
            playerAnimations.SetPlayerWalkAnimation(false);
            ActivateInteractive();
        }
    }

    public void HandleMovement()
    {
        Vector3 movementDirection = new(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        if (playerMovement.IsGrounded())
        {
            playerMovement.MovePlayer(movementDirection);

        }
        playerMovement.RotatePlayer(movementDirection);
    }
    internal void RotateTowardsInteractive()
    {
        Vector3 direction = interactive.transform.position -transform.position;
        playerMovement.RotatePlayer(direction);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactive"))
        {
            interactive = other.GetComponent<Interactive>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactive"))
        {
            interactive = null;
        }
    }

    public void RaiseOpenItemMenuEvent() => OpenItemMenu?.Invoke();
    public void RaiseNavigateShopEvent() => NavigateShop?.Invoke();
    public void ReturnControls()
    {
        if (GetCurrentState() is Conversing)
        {
            SetState(new ExplorationIdle());
            CameraController.Instance.SetState(new PlayerCamera());
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

