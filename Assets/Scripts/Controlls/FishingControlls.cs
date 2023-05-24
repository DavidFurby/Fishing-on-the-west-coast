using System.Collections;
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
            StartCoroutine(WaitForSwingAnimation());
        }
    }
    IEnumerator WaitForSwingAnimation()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Reverse Swing"))
        {
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        isFishing = true;
        bait.thrown = true;
    }



    private void ChargeThrow()
    {

        if (!isSwingingRod)
        {
            isSwingingRod = true;
            animator.Play("Swing");
            bait.reelingIn = true;
        }
        if (bait.throwPower < 50)
            bait.throwPower++;
    }

}
