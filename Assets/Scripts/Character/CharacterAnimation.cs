using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CharacterAnimation : MonoBehaviour
{
    internal Animator animator;
    protected CharacterManager manager;
    internal List<Gesture> gestures = new();
    private Gesture activeGesture;

    internal void Initialize(CharacterManager manager)
    {
        this.manager = manager;
    }
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        print(animator.runtimeAnimatorController != null);
    }
    private void Update()
    {
        CheckIfGestureIsDone();
    }
    internal void SetGestures()
    {
        foreach (GestureName name in System.Enum.GetValues(typeof(GestureName)))
        {
            Expression expression = name switch
            {
                GestureName.Wave => manager.expression.listOfExpressions.FirstOrDefault((Expression expression) => expression.Name == ExpressionName.Happy),
                GestureName.Point => manager.expression.listOfExpressions.FirstOrDefault((Expression expression) => expression.Name == ExpressionName.Shocked),
                GestureName.Mope => manager.expression.listOfExpressions.FirstOrDefault((Expression expression) => expression.Name == ExpressionName.Sad),
                _ => manager.expression.listOfExpressions[0],
            };
            gestures.Add(new Gesture(name, expression));
        }
    }

    public void TriggerWalkAnimation()
    {
        if (animator != null && animator.gameObject.activeSelf)
        {
            if (animator.runtimeAnimatorController != null)
            {
                Debug.Log("Animator Controller Exists");
                animator.CrossFade("Walk", 0.3f, 0);
            }
            else
            {
                Debug.Log("Animator Controller is NULL");
            }
        }
    }
    public void TriggerIdleAnimation()
    {
        if (animator != null)
        {
            animator.CrossFade("Idle", 0, 0);
        }
    }
    public void TriggerGesture(GestureName gestureName, bool active)
    {
        Gesture gesture = gestures.FirstOrDefault((Gesture gesture) => gesture.Name == gestureName);

        if (activeGesture == null && animator != null && gesture != null)
        {
            print(activeGesture);
            animator.CrossFade(gesture.Name.ToString(), 0f);
            activeGesture = gesture;
            manager.expression.TriggerExpression(gesture.expression.Name, active);
        }
    }
    public void CheckIfGestureIsDone()
    {
        if (activeGesture != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(activeGesture.Name.ToString()) && stateInfo.normalizedTime > 1)
            {
                print("done");
                animator.CrossFade("Idle", 0.3f, 0);
                activeGesture = null;
            }
        }
    }
}
public class Gesture
{
    public GestureName Name;
    public Expression expression;

    public Gesture(GestureName name, Expression expression)
    {
        Name = name;
        this.expression = expression;
    }
}
public enum GestureName
{
    Wave,
    Jump,
    Sit,
    Dance,
    Clap,
    Point,
    Mope,
}