using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingMiniGame : MonoBehaviour
{
    [SerializeField] private Scrollbar reelingBalance;
    [SerializeField] private FishingController fishingControlls;
    [SerializeField] private MusicController musicController;
    [SerializeField] private Scrollbar castingPowerBalance;
    private List<FishDisplay> fishesOnHook;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (fishingControlls.stateMachine.GetCurrentState() is ReelingFish)
        {
            CalculateBalance();
            BalanceLost();
            BalanceControls();
            HandleBalanceColor();
        }
        else if (fishingControlls.stateMachine.GetCurrentState() is Charging)
        {
            ChargingBalance();
        }
    }

    #region Balance

    //Change balance value based on fishSize
    private void CalculateBalance()
    {
        if (fishesOnHook != null)
        {
            float weight = 0f;

            for (int i = 0; i < fishesOnHook.Count; i++)
            {
                weight += fishesOnHook[i].fish.size;
            }
            float targetValue;
            if (Random.value < 0.5)
            {
                targetValue = reelingBalance.value - Random.Range(0, weight * downwardForce);
            }
            else
            {
                targetValue = reelingBalance.value + Random.Range(0, weight * upwardForce);
            }

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
    private void HandleBalanceColor()
    {
        float colorValue = Mathf.Abs(reelingBalance.value - 0.5f) * 2;
        reelingBalance.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.red, colorValue);
    }

    //Control balance by pressing the keys
    private void BalanceControls()
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
    private void BalanceLost()
    {
        if (reelingBalance.value <= 0f || reelingBalance.value >= 1f)
        {
            EndBalanceMiniGame();
            fishingControlls.LoseCatch();
        }
    }

    #endregion

    public void StartBalanceMiniGame(List<FishDisplay> fishes)
    {
        reelingBalance.gameObject.SetActive(true);
        fishesOnHook = fishes;
        InvokeRepeating(nameof(AddForce), 2, 2);
        musicController.PlayMiniGameMusic();
    }

    public void EndBalanceMiniGame()
    {
        reelingBalance.value = 0.5f;
        fishesOnHook = null;
        reelingBalance.gameObject.SetActive(false);
        musicController.StopFishingMiniGameMusic();
    }

    public void SetChargingBalance(bool active)
    {
        castingPowerBalance.gameObject.SetActive(active);
    }

    private void ChargingBalance()
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