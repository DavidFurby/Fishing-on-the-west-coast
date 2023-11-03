using UnityEngine;
public class CatchSummaryHandlers : MonoBehaviour

{
    private DialogManager dialogManager;
    private void Start()
    {
        dialogManager = FindAnyObjectByType<DialogManager>();
    }
    public void StartSummary(Fish fish)
    {
        if (dialogManager != null)
        {
            SetCatchSummaryHandler(fish);
            dialogManager.StartDialog("CatchSummary");
        }
    }

    public void SetCatchSummaryHandler(Fish catchResult)
    {
        dialogManager.RemoveHandler("setCatchSummary");
        
        dialogManager.AddHandler("setCatchSummary", () =>
        {
            dialogManager.SetVariableValue("$catchName", catchResult.name);
            dialogManager.SetVariableValue("$catchSize", $"Size: {catchResult.size:F2} cm");
            dialogManager.SetVariableValue("$catchDescription", catchResult.description);

        });
    }
}
