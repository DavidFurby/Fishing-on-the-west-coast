using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishCount : MonoBehaviour
{
    private TextMeshProUGUI fishText;

    private void Start()
    {
        fishText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        fishText.text = $"Total Fishes: {MainManager.Instance.TotalCatches}";
    }
}
