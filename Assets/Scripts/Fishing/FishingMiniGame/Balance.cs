using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Balance : MonoBehaviour
{
    [SerializeField] private Scrollbar reelingBalance;
    private MusicController musicController;
    private const float DEFAULT_FORCE = 0.0005f;
    private float downwardForce = DEFAULT_FORCE;
    private float upwardForce = DEFAULT_FORCE;
    void Start()
    {
        musicController = FindAnyObjectByType<MusicController>();
        reelingBalance.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        SubscribeEvents();
    }

    void OnDisable()
    {
        UnsubscribeEvents();
    }
    private void SubscribeEvents()
    {

        FishingController.OnEnterReelingFish += StartBalanceMiniGame;
        FishingController.OnReelingFish += MiniGameHandler;
        FishingController.OnExitReelingFish += EndBalanceMiniGame;
    }

    private void UnsubscribeEvents()
    {
        FishingController.OnEnterReelingFish -= StartBalanceMiniGame;
        FishingController.OnReelingFish -= MiniGameHandler;
        FishingController.OnExitReelingFish -= EndBalanceMiniGame;
    }

    public void MiniGameHandler()
    {
        CalculateBalance();
        BalanceLost();
        BalanceControls();
        HandleBalanceColor();
    }
    #region Balance
    private void CalculateBalance()
    {
        if (FishingController.Instance.fishesOnHook.Count > 0)
        {
            float weight = 0f;

            foreach (var fish in FishingController.Instance.fishesOnHook)
            {
                weight += fish.fish.size;
            }
            float targetValue = Random.value < 0.5 ? reelingBalance.value - Random.Range(0, weight * downwardForce) : reelingBalance.value + Random.Range(0, weight * upwardForce);

            targetValue = Mathf.Clamp(targetValue, 0f, 1f);
            reelingBalance.value = Mathf.Lerp(reelingBalance.value, targetValue, Time.deltaTime * 1000f);
        }
    }

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
    private void HandleBalanceColor()
    {
        float colorValue = Mathf.Abs(reelingBalance.value - 0.5f) * 2;
        reelingBalance.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.red, colorValue);
    }
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
    private void BalanceLost()
    {
        if (reelingBalance.value <= 0f || reelingBalance.value >= 1f)
        {
            FishingController.Instance.LoseCatch();
        }
    }
    #endregion
    private void StartBalanceMiniGame()
    {
        reelingBalance.gameObject.SetActive(true);
        InvokeRepeating(nameof(AddForce), 2, 2);
        musicController.PlayMiniGameMusic();
    }

    private void EndBalanceMiniGame()
    {
        if (reelingBalance != null)
        {
            reelingBalance.value = 0.5f;
            reelingBalance.gameObject.SetActive(false);
            musicController.StopFishingMiniGameMusic();
        }
    }
    IEnumerator ResetForce()
    {
        yield return new WaitForSeconds(1);
        downwardForce = DEFAULT_FORCE;
        upwardForce = DEFAULT_FORCE;
    }
}