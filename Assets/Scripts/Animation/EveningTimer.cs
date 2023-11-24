using UnityEngine;
[RequireComponent(typeof(TransitArea))]
public class EveningTimer : MonoBehaviour
{
    TransitArea transitArea;
    Animator sunAnimator;

    void OnEnable()
    {
        PlayerEventController.OnEnterIdle += StartSunTimer;
    }
    void Start()
    {
        transitArea = GetComponent<TransitArea>();
        sunAnimator = GetComponent<Animator>();
    }

    void OnDestroy()
    {
        PlayerEventController.OnEnterIdle -= StartSunTimer;
    }
    public void EndDay()
    {
        MainManager.Instance.Days++;
        transitArea.Transition();
    }
    public void StartSunTimer()
    {
        sunAnimator.Play("Evening Light");
    }
}
