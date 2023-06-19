
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishingCanvas : MonoBehaviour
{
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
        fishText.text = "Total Fishes:" + MainManager.Instance.game.Fishes;
    }
}
