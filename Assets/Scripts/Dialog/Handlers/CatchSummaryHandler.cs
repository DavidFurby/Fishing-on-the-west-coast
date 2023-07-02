using UnityEngine;

public class CatchSummaryHandler : MonoBehaviour

{
    [SerializeField] private DialogManager dialogManager;


    public void SetCatchSummaryHandler(Catch catchResult)
    {
        dialogManager.RemoveHandler("setCatchSummary");
        dialogManager.AddCommandHandler("setCatchSummary", () =>
        {
            dialogManager.SetVariableValue("setCatchName", catchResult.CatchName);
            dialogManager.SetVariableValue("setCatchSize", catchResult.Size.ToString());
            dialogManager.SetVariableValue("setCatchDescription", catchResult.Description);

        });
    }
}
