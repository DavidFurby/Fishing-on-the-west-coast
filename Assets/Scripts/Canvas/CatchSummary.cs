using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CatchSummary : MonoBehaviour
{
    [SerializeField] private GameObject catchSummary;
    [SerializeField] private TextMeshProUGUI catchName;
    [SerializeField] private TextMeshProUGUI Size;
    [SerializeField] private TextMeshProUGUI isNew;
    [SerializeField] private FishingControlls fishingControlls;

    private void Start()
    {
        catchSummary.SetActive(false);

    }
    private void Update()
    {
        if (catchSummary.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            SetSummaryActive();
        }
    }


    public void PresentSummary(GameObject fish)
    {
        if (fish.GetComponent<Fish>())
        {
            Fish fishData = fish.GetComponent<Fish>();

            SetSummaryActive();
            catchName.text = fish.GetComponent<Fish>().FishName;
            Size.text += fish.GetComponent<Fish>().Size;
            if (!MainManager.Instance.game.Catches.Contains(fishData))
            {
                MainManager.Instance.game.Catches.SetValue(fishData, MainManager.Instance.game.Catches.Count() - 1);
            }

            isNew.gameObject.SetActive(true);

        }

    }

    public void SetSummaryActive()
    {
        catchSummary.SetActive(!catchSummary.activeSelf);
        fishingControlls.SetControlsActive();
    }

}
