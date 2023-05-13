using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    [SerializeField] int movementSpeed;
    [SerializeField] float rotationSpeed;
    private bool isWithinTriggerArea;
    private Interactible interactible;
    public bool pauseControlls = false;

    void Update()
    {
        if (!pauseControlls)
        {
            HandleInput();

        }
    }
    private void FixedUpdate()
    {
        if (!pauseControlls)
        {
            HandleMovement();
        }

    }
    //Handle player input
    private void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        ActivateInteractible();

    }
    //Handle player movement and rotation
    private void HandleMovement()
    {
        Vector3 movementDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
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
        if (Input.GetKeyDown(KeyCode.Space) && isWithinTriggerArea)
        {
            if (interactible != null)
            {
                interactible.isActivated = true;
            }
        }
    }
}
