using Unity.VisualScripting;
using UnityEngine;

public class CatchSummaryHandlers : MonoBehaviour

{
    [SerializeField] private DialogManager dialogManager;

    public void StartSummar(FishDisplay catchResult)
    {
        if (dialogManager != null)
        {
            dialogManager.EndDialog();
            SetCatchSummaryHandler(catchResult.fish);
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
