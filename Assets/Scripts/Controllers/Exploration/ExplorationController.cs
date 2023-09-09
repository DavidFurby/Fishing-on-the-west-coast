using UnityEngine;
using System;

public class ExplorationController : ExplorationStateMachine
{

    private PlayerAnimations playerAnimations;
    private Interactive interactive;
    private readonly int movementSpeed = 10;
    public static event Action NavigateShop;
    public static event Action OpenItemMenu;

    private void OnEnable()
    {
        DialogManager.OnEndDialog += RaiseEndDialog;
        FishingSpot.StartFishing += RaiseStartFishing;
    }
    void Start()
    {
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
        SetState(new ExplorationIdle(this));

    }

    private void OnDisable()
    {
        DialogManager.OnEndDialog -= RaiseEndDialog;
        FishingSpot.StartFishing -= RaiseStartFishing;
    }
    //Handle player input
    public void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        playerAnimations.SetPlayerWalkAnimation(horizontalInput != 0 || verticalInput != 0);

        if (Input.GetKeyDown(KeyCode.Space) && interactive != null)
        {
            playerAnimations.SetPlayerWalkAnimation(false);
            ActivateInteractive();
        }
    }

    //Handle player movement and rotation
    public void HandleMovement()
    {
        Vector3 movementDirection = new(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        MovePlayer(movementDirection);
        RotatePlayer(movementDirection);
    }

    //Move player in specified direction
    private void MovePlayer(Vector3 movementDirection)
    {
        transform.Translate(movementSpeed * Time.fixedDeltaTime * movementDirection, Space.World);
    }

    //Rotate the player to face the specified direction
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

    //Called when player exists trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactive"))
        {
            interactive = null;
        }
    }

    public void RaiseOpenItemMenuEvent() => OpenItemMenu?.Invoke();

    public void RaiseNavigateShopEvent() => NavigateShop?.Invoke();

    public void RaiseEndDialog() => SetState(new ExplorationIdle(this));

    public void RaiseStartFishing() => SetState(new StartFishing(this));

    //Activates interactive object if player is within trigger area and button is pressed
    private void ActivateInteractive()
    {
        SetState(new Interacting(this));
        interactive.StartInteraction();
    }
}
