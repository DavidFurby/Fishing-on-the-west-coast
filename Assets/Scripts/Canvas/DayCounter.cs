using TMPro;
using UnityEngine;

public class DayCounter : MonoBehaviour
{
    private int dayCount;
    private TextMeshProUGUI CounterText;
    // Start is called before the first frame update
    void Start()
    {
        CounterText = GetComponent<TextMeshProUGUI>();
        if (MainManager.Instance != null)
        {
            dayCount = MainManager.Instance.Days;
        }
        CounterText.text = "Days:" + dayCount;
    }
}
