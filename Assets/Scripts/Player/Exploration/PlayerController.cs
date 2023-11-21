using UnityEngine;
using System;
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : FishingController
{
    public static PlayerController Instance { get; private set; }
    private PlayerAnimations playerAnimations;
    private Interactive interactive;
    private PlayerMovement playerMovement;
    public static event Action OnNavigateShop;
    public static event Action OnOpenItemMenu;

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
        CharacterDialog.OnStartConversation += (_) => SetState(new PlayerInDialog());
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
        CharacterDialog.OnStartConversation -= (_) => SetState(new PlayerInDialog());
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
        Vector3 direction = interactive.transform.position - transform.position;
        playerMovement.RotatePlayer(direction);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (IsInteractive(other))
        {
            interactive = other.GetComponent<Interactive>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsInteractive(other))
        {
            interactive = null;
        }
    }

    private bool IsInteractive(Collider other)
    {
        return other.CompareTag("Interactive") || other.CompareTag("Character");
    }

    public void RaiseOpenItemMenuEvent() => OnOpenItemMenu?.Invoke();
    public void RaiseNavigateShopEvent() => OnNavigateShop?.Invoke();

    //Return to idle state only if current state is dialog
    public void ReturnControls()
    {
        if (GetCurrentState() is PlayerInDialog)
        {
            SetState(new ExplorationIdle());
            CameraController.Instance.EnterPlayerState();
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

