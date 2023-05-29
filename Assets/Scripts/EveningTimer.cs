using UnityEngine;

public class EveningTimer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TransitArea transitArea;
    [SerializeField] MainManager mainManager;

    public void EndDay()
    {
        Debug.Log("End day");
        mainManager.game.days++;
        transitArea.Transition();
    }
}
