using TMPro;
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
        if (fish.GetComponent<FishMovement>())
        {
            SetSummaryActive();
            catchName.text = fish.name;
            Size.text += fish.GetComponent<FishMovement>().size.ToString();
            isNew.gameObject.SetActive(true);

        }

    }

    public void SetSummaryActive()
    {
        catchSummary.SetActive(!catchSummary.activeSelf);
        fishingControlls.SetControlsActive();
    }

}
