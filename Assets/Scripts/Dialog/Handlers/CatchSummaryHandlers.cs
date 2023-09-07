using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;
public class CatchSummaryHandlers : MonoBehaviour

{
     private DialogManager dialogManager;
     private readonly CatchSummaryView catchSummaryView;
     void Start()
     {
        dialogManager = FindAnyObjectByType<DialogManager>();
     }
    public void StartSummary(Fish fish)
    {
        DialogueViewBase[] dialogueViewBases= new DialogueViewBase[1];
        dialogueViewBases[0] = catchSummaryView;
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
