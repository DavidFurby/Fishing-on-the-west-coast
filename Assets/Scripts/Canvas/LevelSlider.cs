using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSlider : MonoBehaviour
{
    private Slider levelSlider;
    private TextMeshProUGUI currentLevelText;
    // Start is called before the first frame update
    void Start()
    {
        currentLevelText = transform.Find("CurrentLevel").GetComponent<TextMeshProUGUI>();
        levelSlider = transform.Find("LevelSlider").GetComponent<Slider>();
        levelSlider.gameObject.SetActive(true);
        SetLevel();
    }

    public void SetLevel()
    {
        levelSlider.maxValue = MainManager.Instance.playerLevel.requiredExp;
        levelSlider.value = MainManager.Instance.playerLevel.Exp;
        currentLevelText.text = MainManager.Instance.playerLevel.Level.ToString();
    }
}
