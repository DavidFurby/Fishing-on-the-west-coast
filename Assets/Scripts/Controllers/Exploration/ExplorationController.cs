using UnityEngine;
using System;

public class ExplorationController : ExplorationStateMachine
{
    [SerializeField] private int movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private PlayerAnimations playerAnimations;


    private bool isWithinTriggerArea;
    private Interactive interactive;

    public static event Action NavigateShop;
    public static event Action OpenItemMenu;



    void Start()
    {
        SetState(new ExplorationIdle(this));

    }

    //Handle player input
    public void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            playerAnimations.SetPlayerWalkAnimation(true);
        }
        else
        {
            playerAnimations.SetPlayerWalkAnimation(false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isWithinTriggerArea && interactive != null)
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
        {
            transform.rotation = Quaternion.LookRotation(movementDirection);
        }
    }

    //Called when player enters trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactive"))
        {
            isWithinTriggerArea = true;
            interactive = other.GetComponent<Interactive>();
        }
    }

    //Called when player exists trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactive"))
        {
            isWithinTriggerArea = false;
            interactive = null;
        }
    }
    public void RaiseOpenItemMenuEvent()
    {
        OpenItemMenu?.Invoke();
    }
    public void RaiseNavigateShopEvent()
    {
        NavigateShop?.Invoke();
    }

    //Activates interactive object if player is within trigger area and button is pressed
    private void ActivateInteractive()
    {
        SetState(new Interacting(this));
        interactive.CheckActivated();
    }
}
