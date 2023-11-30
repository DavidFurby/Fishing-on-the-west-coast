using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CharacterGesture : MonoBehaviour
{
    protected CharacterAnimationController controller;
    internal List<Gesture> gestures = new();

    internal void Initialize(CharacterAnimationController controller)
    {
        this.controller = controller;
    }
    void Start() {
        SetGestures();
    }

    internal void SetGestures()
    {
        foreach (GestureName name in System.Enum.GetValues(typeof(GestureName)))
        {
            Expression expression = name switch
            {
                GestureName.Wave => controller.expression.listOfExpressions.FirstOrDefault((Expression expression) => expression.Name == ExpressionName.Happy),
                GestureName.Point => controller.expression.listOfExpressions.FirstOrDefault((Expression expression) => expression.Name == ExpressionName.Shocked),
                GestureName.Mope => controller.expression.listOfExpressions.FirstOrDefault((Expression expression) => expression.Name == ExpressionName.Sad),
                _ => controller.expression.listOfExpressions[0],
            };
            gestures.Add(new Gesture(name, expression));
        }
    }

    public void TriggerWalkAnimation()
    {
        if (controller.animator != null && controller.animator.gameObject.activeSelf)
        {
            controller.animator.CrossFade("Walk", 0.3f, 0);
        }
    }
    public void TriggerIdleAnimation()
    {
        if (controller.animator != null)
        {
            controller.animator.CrossFade("Idle", 0, 0);
        }
    }
    public void TriggerGesture(GestureName gestureName)
    {
        Gesture gesture = gestures.FirstOrDefault((Gesture gesture) => gesture.Name == gestureName);
        if (controller.animator != null && gesture != null)
        {
            controller.animator.CrossFade(gesture.Name.ToString(), 0f, 0);
            controller.expression.TriggerExpression(gesture.expression.Name);
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