using UnityEngine;
using System;

public class PlayerController : FishingController
{
    public static PlayerController Instance { get; private set; }
    private PlayerAnimations playerAnimations;
    private Interactive interactive;
    private const int MovementSpeed = 10;
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
    }

    private void InitializeComponents()
    {
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
        SetState(new ExplorationIdle());
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        WaterCollision.OnEnterSea += EnterSea;
        FishingSpot.StartFishing += () => SetState(new FishingIdle());
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
        WaterCollision.OnEnterSea -= EnterSea;
        FishingSpot.StartFishing -= () => SetState(new FishingIdle());
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
        Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        MovePlayer(movementDirection);
        RotatePlayer(movementDirection);
    }

    private void MovePlayer(Vector3 movementDirection)
    {
        transform.Translate(MovementSpeed * Time.fixedDeltaTime * movementDirection, Space.World);
    }

    private void RotatePlayer(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(movementDirection);
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
        if (GetCurrentState() is Conversing || GetCurrentState() is Shopping)
        {
            SetState(new ExplorationIdle());
            CameraController.Instance.SetState(new PlayerCamera());
        }
    }

    public void RaiseStartFishing() => SetState(new FishingIdle());

    private void ActivateInteractive()
    {
        SetState(new Interacting());
        interactive.StartInteraction();
    }
}
