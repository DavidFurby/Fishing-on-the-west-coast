using UnityEngine;

public class FishingControlls : MonoBehaviour
{
    [SerializeField] CatchArea catchArea;
    public bool isFishing = false;
    [SerializeField] Bait bait;
    [SerializeField] GameObject fishingRod;
    private bool isSwingingRod;
    [SerializeField] Animator animator;
    // Update is called once per frame
    void Update()
    {
        if (isFishing)
        {
            ReturnBait();

        }
        else
        {
            StartFishing();

        }
    }

    private void ReturnBait()
    {
        if (catchArea.isInCatchArea)
        {
            Debug.Log("Get em");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (catchArea.isInCatchArea)
            {
                Debug.Log("Catch");
            }

            isFishing = false;
        }

    }

    private void StartFishing()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ChargeThrow();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.Play("Reverse Swing");
            isSwingingRod = false;
            isFishing = true;
            bait.ThrowBait();
        }
    }

    private void ChargeThrow()
    {

        if (!isSwingingRod)
        {
            isSwingingRod = true;
            animator.Play("Swing");
        }
        if (bait.throwPower < 200)
            bait.throwPower++;

    }

}
