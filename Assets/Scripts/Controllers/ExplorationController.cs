using UnityEngine;

public class ExplorationController : MonoBehaviour
{
    [SerializeField] private int movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private PlayerAnimations playerAnimations;

    private bool isWithinTriggerArea;
    private Interactible interactible;
    public PlayerStatus playerStatus;

    public enum PlayerStatus
    {
        StandBy,
        Interacting,
        Shopping
    }

    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    //Handle player input
    private void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (playerStatus == PlayerStatus.StandBy)
        {
            if (horizontalInput != 0 || verticalInput != 0)
            {
                playerAnimations.SetPlayerWalkAnimation(true);
            }
            else
            {
                playerAnimations.SetPlayerWalkAnimation(false);
            }

            if (Input.GetKeyDown(KeyCode.Space) && isWithinTriggerArea && interactible != null)
            {
                playerAnimations.SetPlayerWalkAnimation(false);
                ActivateInteractible();
            }
        }
    }

    //Handle player movement and rotation
    private void HandleMovement()
    {
        if (playerStatus == PlayerStatus.StandBy)
        {
            Vector3 movementDirection = new(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            MovePlayer(movementDirection);
            RotatePlayer(movementDirection);
        }
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
        if (other.CompareTag("Interactible"))
        {
            isWithinTriggerArea = true;
            interactible = other.GetComponent<Interactible>();
        }
    }

    //Called when player exists trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactible"))
        {
            isWithinTriggerArea = false;
            interactible = null;
        }
    }

    //Activates interactible object if player is within trigger area and button is pressed
    private void ActivateInteractible()
    {
        SetPlayerStatus(PlayerStatus.Interacting);
        interactible.CheckActivated();
    }

    public void SetPlayerStatus(PlayerStatus playerStatus)
    {
        this.playerStatus = playerStatus;
    }
}