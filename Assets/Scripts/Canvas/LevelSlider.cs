using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSlider : MonoBehaviour
{
    [SerializeField] private Slider levelSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    // Start is called before the first frame update
    void Start()
    {
        levelSlider.gameObject.SetActive(true);
        SetLevel();
    }

    public void SetLevel() {
        levelSlider.maxValue = MainManager.Instance.game.playerLevel.requiredExp;
        levelSlider.value = MainManager.Instance.game.playerLevel.Exp;
        levelText.text = MainManager.Instance.game.playerLevel.Level.ToString();
    }
}
