using System.Collections;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float swimSpeed;
    public Vector3 direction;
    [SerializeField] Camera mainCamera;
    private float destroyTimer = 4;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(nameof(Swim));
    }

    // Update is called once per frame
    void Update()
    {
        FloatSpeed();
        DestroyOnExit();

    }
    // Method to move fish based on speed and direction
    void FloatSpeed()
    {
        transform.position += speed * Time.fixedDeltaTime * direction;
    }
    // Coroutine to add force to fish's rigidbody
    IEnumerator Swim()
    {
        rb.AddForce(speed * Time.fixedDeltaTime * direction);
        yield return new WaitForSeconds(swimSpeed);
    }
    // Method to check if fish should be destroyed based on destroy timer and position
    void DestroyOnExit()
    {
        destroyTimer -= Time.deltaTime;
        if (destroyTimer < 0)
        {
            Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
            if (viewPos.x < 0 || viewPos.x > 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
