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
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
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

    public void TriggerWalkAnimation(bool active)
    {
        if (animator != null)
        {
            animator.SetBool("walking", active);
        }
    }
    public void TriggerGesture(GestureName gestureName, bool active)
    {
        Gesture gesture = gestures.FirstOrDefault((Gesture gesture) => gesture.Name == gestureName);

        if (activeGesture == null && animator != null && gesture != null)
        {
            animator.SetBool(gesture.Name.ToString().ToLower(), active);
            activeGesture = gesture;
            manager.expression.TriggerExpression(gesture.expression.Name, active);
        }
    }
    public void CheckIfGestureIsDone()
    {
        if (activeGesture != null && animator.GetBool(activeGesture.Name.ToString().ToLower()))
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime > 1 && !animator.IsInTransition(0))
            {
                animator.SetBool(activeGesture.Name.ToString().ToLower(), false);
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