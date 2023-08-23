using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingMiniGame : MonoBehaviour
{
    [SerializeField] private Scrollbar reelingBalance;
    [SerializeField] private FishingController fishingSystem;
    [SerializeField] private MusicController musicController;
    [SerializeField] private Scrollbar castingPowerBalance;
    private const float DEFAULT_FORCE = 0.0005f;
    private float downwardForce = DEFAULT_FORCE;
    private float upwardForce = DEFAULT_FORCE;
    private bool castingPowerDirection = true;
    [HideInInspector] public float castPower = 1;



    // Start is called before the first frame update
    void Start()
    {
        reelingBalance.gameObject.SetActive(false);
        castingPowerBalance.gameObject.SetActive(false);
        FishingController.OnChargeRelease += SetChargingBalance;
    }

    #region Balance

    // Change balance value based on fishSize
    public void CalculateBalance()
    {
        if (fishingSystem.fishesOnHook.Count > 0)
        {
            float weight = 0f;

            // Use a foreach loop to iterate over the fishesOnHook list
            foreach (var fish in fishingSystem.fishesOnHook)
            {
                weight += fish.fish.size;
            }

            // Use a ternary operator to simplify the conditional assignment of targetValue
            float targetValue = Random.value < 0.5 ? reelingBalance.value - Random.Range(0, weight * downwardForce) : reelingBalance.value + Random.Range(0, weight * upwardForce);

            targetValue = Mathf.Clamp(targetValue, 0f, 1f);
            reelingBalance.value = Mathf.Lerp(reelingBalance.value, targetValue, Time.deltaTime * 1000f);
        }
    }

    //Add weight towards the direction the bait moving towards
    private void AddForce()
    {
        if (reelingBalance.value >= 0.5)
        {
            upwardForce = 0.0005f;
        }
        else
        {
            downwardForce = 0.0005f;
        }
        StartCoroutine(ResetForce());
    }
    //Reset the added force after 
    IEnumerator ResetForce()
    {
        yield return new WaitForSeconds(1);
        downwardForce = DEFAULT_FORCE;
        upwardForce = DEFAULT_FORCE;
    }

    //Change the color of the scrollbar based on its value
    public void HandleBalanceColor()
    {
        float colorValue = Mathf.Abs(reelingBalance.value - 0.5f) * 2;
        reelingBalance.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.red, colorValue);
    }

    //Control balance by pressing the keys
    public void BalanceControls()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            reelingBalance.value -= 0.005f;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            reelingBalance.value += 0.005f;
        }
    }

    // Check if balance is lost and therefore lose the catch
    public void BalanceLost()
    {
        if (reelingBalance.value <= 0f || reelingBalance.value >= 1f)
        {
            fishingSystem.LoseCatch();
        }
    }

    #endregion

    public void StartBalanceMiniGame()
    {
        reelingBalance.gameObject.SetActive(true);
        InvokeRepeating(nameof(AddForce), 2, 2);
        musicController.PlayMiniGameMusic();
    }

    public void EndBalanceMiniGame()
    {
        if (reelingBalance != null)
        {
            reelingBalance.value = 0.5f;
            reelingBalance.gameObject.SetActive(false);
            musicController.StopFishingMiniGameMusic();
        }
    }

    public void SetChargingBalance(bool active)
    {
        castingPowerBalance.gameObject.SetActive(active);
    }

    public void ChargingBalance()
    {
        if (castingPowerBalance.gameObject.activeSelf)
        {
            castPower = Mathf.Min(castingPowerBalance.value, 1 - castingPowerBalance.value) + 1f;
            castingPowerBalance.value += castingPowerDirection ? 0.05f : -0.05f;
            if (castingPowerBalance.value >= 1)
            {
                castingPowerDirection = false;
            }
            else if (castingPowerBalance.value <= 0)
            {
                castingPowerDirection = true;
            }
        }

    }
}