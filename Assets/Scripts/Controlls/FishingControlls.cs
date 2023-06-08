using System.Collections;
using UnityEngine;

public class FishingControlls : MonoBehaviour
{
    [SerializeField] CatchArea catchArea;
    [SerializeField] Bait bait;
    [SerializeField] GameObject fishingRod;
    [SerializeField] Animator animator;
    public float throwPower;
    [SerializeField] AudioSource castSound;

    public enum GetFishingStatus
    {
        StandBy,
        Casting,
        Fishing,
        Reeling
    }
    public GetFishingStatus fishingStatus;

    private void Start()
    {
        fishingStatus = GetFishingStatus.StandBy;

    }
    // Update is called once per frame
    void Update()
    {
        if (fishingStatus == GetFishingStatus.Fishing)
        {
            ReelInBait();
        }
        else if (fishingStatus == GetFishingStatus.StandBy)
        {
            StartFishing();
        }
    }

    private void ReelInBait()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (catchArea.isInCatchArea)
            {
                catchArea.CatchFish();
            }
            fishingStatus = GetFishingStatus.Reeling;
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
            castSound.Play();
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
        fishingStatus = GetFishingStatus.Casting;
    }



    private void ChargeThrow()
    {
        animator.Play("Swing");
        if (throwPower < 100)
            throwPower++;
    }


}
