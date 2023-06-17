using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject fishingMiniGame;
    [SerializeField] private Scrollbar balance;
    [SerializeField] private FishingControlls fishingControlls;
    private Fish fish;
    private const float DEFAULT_FORCE = 0.0005f;
    private float downwardForce = DEFAULT_FORCE;
    private float upwardForce = DEFAULT_FORCE;


    // Start is called before the first frame update
    void Start()
    {
        fishingMiniGame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (fishingControlls.fishingStatus == FishingControlls.FishingStatus.ReelingFish)
        {
            CalculateBalance();
            BalanceLost();
            BalanceControls();
            HandleBalanceColor();
        }
    }

    #region Balance

    //Change balance value based on fishSize
    private void CalculateBalance()
    {
        if (fish != null)
        {
            float targetValue;
            if (Random.value < 0.5)
            {
                targetValue = balance.value -= Random.Range(0, fish.Size * downwardForce);
            }
            else
            {
                targetValue = balance.value += Random.Range(0, fish.Size * upwardForce);
            }
            targetValue = Mathf.Clamp(targetValue, 0f, 1f);
            balance.value = Mathf.Lerp(balance.value, targetValue, Time.deltaTime * 1000f);
        }
    }
    //Add weight towards the balance is moving towards
    private void AddForce()
    {
        if (balance.value >= 0.5)
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
        float colorValue = Mathf.Abs(balance.value - 0.5f) * 2;
        balance.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.red, colorValue);
    }

    //Control balance by pressing the keys
    private void BalanceControls()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            balance.value -= 0.005f;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            balance.value += 0.005f;
        }
    }

    // Check if balance is lost and therefore lose the catch
    private void BalanceLost()
    {
        if (balance.value <= 0f || balance.value >= 1f)
        {
            fishingControlls.LoseCatch();
        }
    }

    #endregion

    public void StartFishingMiniGame(Fish fish)
    {
        fishingMiniGame.SetActive(true);
        this.fish = fish;
        InvokeRepeating(nameof(AddForce), 2, 2);
    }

    public void EndFishingMiniGame()
    {
        balance.value = 0.5f;
        fish = null;
        fishingMiniGame.SetActive(false);
    }
}