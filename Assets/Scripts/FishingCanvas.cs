
using TMPro;
using UnityEngine;

public class FishingCanvas : MonoBehaviour
{
    public float fishCount = 0;
    [SerializeField] TextMeshProUGUI fishText;
    // Start is called before the first frame update
    void Update()
    {
        fishText.text = "Total Fishes:" + fishCount;
    }
}
