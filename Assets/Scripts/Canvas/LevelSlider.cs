using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSlider : MonoBehaviour
{
    private Slider levelSlider;
    private TextMeshProUGUI currentLevelText;

    void OnEnable()
    {
        PlayerLevel.OnLevelUp += SetLevel;

    }
    // Start is called before the first frame update
    void Start()
    {
        currentLevelText = transform.Find("CurrentLevel").GetComponent<TextMeshProUGUI>();
        levelSlider = transform.Find("LevelSlider").GetComponent<Slider>();
        levelSlider.gameObject.SetActive(true);
        SetLevel();
    }
    void OnDisable()
    {
        PlayerLevel.OnLevelUp -= SetLevel;
    }
    public void SetLevel()
    {
        levelSlider.maxValue = MainManager.Instance.PlayerLevel.requiredExp;
        levelSlider.value = MainManager.Instance.PlayerLevel.Exp;
        currentLevelText.text = MainManager.Instance.PlayerLevel.Level.ToString();
    }
}
