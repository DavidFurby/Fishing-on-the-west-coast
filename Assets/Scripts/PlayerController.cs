using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private float horizontalInput;
    private float verticalInput;
    public int movementSpeed;
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        Vector3 movementDirection = new(horizontalInput, 0.0f, verticalInput);
        MovePlayer(movementDirection);
        RotatePlayer(movementDirection);

    }

    private void MovePlayer(Vector3 movementDirection)
    {
        transform.Translate(movementSpeed * Time.deltaTime * movementDirection, Space.World);
    }
    private void RotatePlayer(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementDirection);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space) && other.CompareTag("Interactible"))
        {
            TriggerInteractible(other);
        }
    }
    private void TriggerInteractible(Collider other)
    {
        Debug.Log("Pressed");
        Interactible interactible = other.gameObject.GetComponent<Interactible>();
        interactible.triggered = true;
    }
}
