using UnityEngine;
using UnityEngine.UI;

public class FishingMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject fishingMiniGame;
    [SerializeField] private Scrollbar balance;
    [SerializeField] private CatchArea catchArea;
    [SerializeField] private FishingControlls fishingControlls;
    private float fishSize = 0f;

    // Start is called before the first frame update
    void Start()
    {
        fishingMiniGame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (fishingControlls.fishingStatus == FishingControlls.FishingStatus.Reeling)
        {
            if (catchArea.fish != null && fishSize == 0f)
            {
                fishSize = catchArea.fish.GetComponent<Fish>().Size;
                Debug.Log(fishSize);
            }
            CalculateBalance();
            BalanceLost();

            BalanceControls();

            float colorValue = Mathf.Abs(balance.value - 0.5f) * 2;
            balance.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.red, colorValue);
        }
    }
    //Control balance by pressing the keys
    private void BalanceControls()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            balance.value -= 0.05f;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            balance.value += 0.05f;
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
    //Change balance value based on fishSize
    private void CalculateBalance()
    {
        if (catchArea.fish != null)
        {
            if (Random.value < 0.5)
            {
                Debug.Log(Random.Range(0, fishSize));
                balance.value -= Random.Range(0, fishSize / 1000);
            }
            else
            {
                balance.value += Random.Range(0, fishSize / 1000);
            }
            balance.value = Mathf.Clamp(balance.value, 0f, 1f);
        }
    }

    public void SetFishingMiniGame()
    {
        fishingMiniGame.SetActive(!fishingMiniGame.activeSelf);
    }

    public void ResetValues()
    {
        balance.value = 0.5f;
        fishSize = 0f;
    }
}