using Unity.VisualScripting;
using UnityEngine;

public class CatchSummaryHandlers : MonoBehaviour

{
    [SerializeField] private DialogManager dialogManager;

    public void StartSummar(Catch catchResult)
    {
        if (dialogManager != null)
        {
            dialogManager.EndDialog();
            SetCatchSummaryHandler(catchResult);
            dialogManager.StartDialog("CatchSummary");
        }
    }

    public void SetCatchSummaryHandler(Catch catchResult)
    {
        dialogManager.RemoveHandler("setCatchSummary");
        dialogManager.AddCommandHandler("setCatchSummary", () =>
        {
            dialogManager.SetVariableValue("$catchName", catchResult.Name);
            dialogManager.SetVariableValue("$catchSize", $"Size: {catchResult.Size:F2} cm");
            dialogManager.SetVariableValue("$catchDescription", catchResult.Description);

        });
    }
    public void EndSummary()
    {
        dialogManager.EndDialog();
    }
}
