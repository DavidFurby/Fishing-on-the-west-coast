using TMPro;
using UnityEngine;

public class DayCounter : MonoBehaviour
{
    private int dayCount;
    [SerializeField] TextMeshProUGUI counter;
    // Start is called before the first frame update
    void Start()
    {
        if (MainManager.Instance != null)
        {
            dayCount = MainManager.Instance.Days;
        }
        counter.text = "Days:" + dayCount;
    }
}
