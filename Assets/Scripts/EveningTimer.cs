using UnityEngine;

public class EveningTimer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TransitArea transitArea;

    public void EndDay()
    {
        Debug.Log("End day");
        MainManager.Instance.game.days++;
        transitArea.Transition();
    }
}
