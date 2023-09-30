using TMPro;
using UnityEngine;

public class FishCount : MonoBehaviour
{
    private TextMeshProUGUI fishText;

    private void Start()
    {
        fishText = transform.Find("Counter").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        fishText.text = $"Total Fishes: {MainManager.Instance.TotalCatches}";
    }
}
