using Unity.VisualScripting;
using UnityEngine;

public class CatchSummaryHandlers : MonoBehaviour

{
    [SerializeField] private DialogManager dialogManager;

    public void StartSummary(Fish fish)
    {
        if (dialogManager != null)
        {
            dialogManager.EndDialog();
            SetCatchSummaryHandler(fish);
            dialogManager.StartDialog("CatchSummary");
        }
    }

    public void SetCatchSummaryHandler(Fish catchResult)
    {
        dialogManager.RemoveHandler("setCatchSummary");
        dialogManager.AddCommandHandler("setCatchSummary", () =>
        {
            dialogManager.SetVariableValue("$catchName", catchResult.name);
            dialogManager.SetVariableValue("$catchSize", $"Size: {catchResult.size:F2} cm");
            dialogManager.SetVariableValue("$catchDescription", catchResult.description);

        });
    }
    public void EndSummary()
    {
        dialogManager.EndDialog();
    }
}
