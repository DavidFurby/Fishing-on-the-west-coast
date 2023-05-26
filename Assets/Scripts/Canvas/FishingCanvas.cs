
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishingCanvas : MonoBehaviour
{
    public float fishCount = 0;
    [SerializeField] TextMeshProUGUI fishText;
    // Start is called before the first frame update

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Boat")
        {
            fishText.gameObject.SetActive(true);
        }
    }
    void Update()
    {
        fishText.text = "Total Fishes:" + fishCount;
    }
}
