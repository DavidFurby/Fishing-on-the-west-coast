using UnityEngine;

public class FishingControlls : MonoBehaviour
{
    [SerializeField] CatchArea catchArea;
    public bool isFishing = false;
    [SerializeField] Bait bait;
    [SerializeField] GameObject fishingRod;
    private bool isSwingingRod;
    [SerializeField] Animator animator;
    [SerializeField] Rope rope;

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
            rope.forceAdded = true;
        }
    }

    private void ChargeThrow()
    {

        if (!isSwingingRod)
        {
            isSwingingRod = true;
            animator.Play("Swing");
        }
        if (rope.throwPower < 500)
            rope.throwPower++;

    }

}
