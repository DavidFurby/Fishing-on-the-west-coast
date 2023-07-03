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
            Debug.Log(catchResult.Size.ToString());
            dialogManager.SetVariableValue("$catchName", catchResult.CatchName);
            dialogManager.SetVariableValue("$catchSize", catchResult.Size.ToString());
            dialogManager.SetVariableValue("$catchDescription", catchResult.Description);

        });
    }
    public void EndSummary()
    {
        dialogManager.EndDialog();
    }
}
