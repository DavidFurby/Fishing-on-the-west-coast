using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Balance : MonoBehaviour
{
    private Scrollbar reelingBalance;
    private MusicController musicController;
    private const float DEFAULT_FORCE = 0.0005f;
    private float downwardForce = DEFAULT_FORCE;
    private float upwardForce = DEFAULT_FORCE;
    private float weight = 0;

    void Start()
    {
        InitializeComponents();
    }

    void OnEnable()
    {
        SubscribeEvents();
    }

    void OnDestroy()
    {
        UnsubscribeEvents();
    }

    // Initialize components
    private void InitializeComponents()
    {
        reelingBalance = GetComponentInChildren<Scrollbar>();
        musicController = FindAnyObjectByType<MusicController>();
        reelingBalance.gameObject.SetActive(false);
    }

    // Subscribe to events
    private void SubscribeEvents()
    {
        PlayerEventController.OnEnterReelingFish += StartBalanceMiniGame;
        PlayerEventController.OnEnterReelingFish += CalculateWeight;
        PlayerEventController.OnWhileReelingFish += MiniGameHandler;
        PlayerEventController.OnExitReelingFish += EndBalanceMiniGame;
    }

    // Unsubscribe from events
    private void UnsubscribeEvents()
    {
        PlayerEventController.OnEnterReelingFish -= StartBalanceMiniGame;
        PlayerEventController.OnEnterReelingFish -= CalculateWeight;
        PlayerEventController.OnWhileReelingFish -= MiniGameHandler;
        PlayerEventController.OnExitReelingFish -= EndBalanceMiniGame;
    }

    // Handle mini game
    public void MiniGameHandler()
    {
        CalculateBalance();
        BalanceLost();
        BalanceControls();
        HandleBalanceColor();
    }

    #region Balance

    // Calculate balance based on fish weight
    private void CalculateBalance()
    {
        if (PlayerController.Instance.fishesOnHook.Count > 0)
        {
            float targetValue = Random.value < 0.5 ? reelingBalance.value - Random.Range(0, weight * downwardForce) : reelingBalance.value + Random.Range(0, weight * upwardForce);

            targetValue = Mathf.Clamp(targetValue, 0f, 1f);
            reelingBalance.value = Mathf.Lerp(reelingBalance.value, targetValue, Time.deltaTime * 1000f);
        }
    }

    private void CalculateWeight()
    {

        foreach (var fish in PlayerController.Instance.fishesOnHook)
        {
            weight += fish.fish.size;
        }
    }

    // Add force based on balance value
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

    // Handle balance color based on value
    private void HandleBalanceColor()
    {
        float colorValue = Mathf.Abs(reelingBalance.value - 0.5f) * 2;
        reelingBalance.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.red, colorValue);
    }

    // Control balance with user input
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

    // Check if balance is lost
    private void BalanceLost()
    {
        if (reelingBalance.value <= 0f || reelingBalance.value >= 1f)
        {
            PlayerController.Instance.LoseCatch();
        }
    }

    #endregion

    // Start mini game
    private void StartBalanceMiniGame()
    {
        reelingBalance.gameObject.SetActive(true);
        InvokeRepeating(nameof(AddForce), 2, 4);
        musicController.PlayMiniGameMusic();
    }

    // End mini game
    private void EndBalanceMiniGame()
    {
        if (reelingBalance != null)
        {
            weight = 0;
            reelingBalance.value = 0.5f;
            reelingBalance.gameObject.SetActive(false);
            musicController.StopFishingMiniGameMusic();
        }
    }

    // Reset force after a delay
    IEnumerator ResetForce()
    {
        yield return new WaitForSeconds(1);
        downwardForce = DEFAULT_FORCE;
        upwardForce = DEFAULT_FORCE;
    }

    // Get current balance value
    public float GetBalanceValue()
    {
        return reelingBalance.value;
    }
}
