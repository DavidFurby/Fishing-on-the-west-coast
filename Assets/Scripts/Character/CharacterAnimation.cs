using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CharacterAnimation : MonoBehaviour
{
    internal Animator animator;
    protected CharacterManager manager;
    internal List<Gesture> gestures = new();

    internal void Initialize(CharacterManager manager)
    {
        this.manager = manager;
    }
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
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
            animator.CrossFade("Walk", 0.3f, 0);
        }
    }
    public void TriggerIdleAnimation()
    {
        if (animator != null)
        {
            animator.CrossFade("Idle", 0, 0);
        }
    }
    public void TriggerGesture(GestureName gestureName)
    {
        Gesture gesture = gestures.FirstOrDefault((Gesture gesture) => gesture.Name == gestureName);
        if (animator != null && gesture != null)
        {
            animator.CrossFade(gesture.Name.ToString(), 0f, 0);
            manager.expression.TriggerExpression(gesture.expression.Name);
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