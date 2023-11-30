using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterExpression : MonoBehaviour
{
    private CharacterAnimationController controller;
    public List<Expression> listOfExpressions = new();
    private int layerIndex;
    internal void Initialize(CharacterAnimationController controller)
    {
        this.controller = controller;
    }

    void Start()
    {
        layerIndex = controller.animator.GetLayerIndex("Expression Layer");
        SetExpressions();
        SetExpressionMotions();
    }


    internal void SetExpressions()
    {
        foreach (ExpressionName name in System.Enum.GetValues(typeof(ExpressionName)))
        {
            BlendShape[] shapes = name switch
            {
                ExpressionName.Happy => controller.ShapeList.Where((BlendShape shape) => shape.Name == "Blink Happy" || shape.Name == "Mouth Open").ToArray(),
                ExpressionName.Shocked => controller.ShapeList.Where((BlendShape shape) => shape.Name == "Mouth Open").ToArray(),
                ExpressionName.Sad => controller.ShapeList.Where((BlendShape shape) => shape.Name == "Eyes Sad" || shape.Name == "Mouth Sad").ToArray(),
                _ => controller.ShapeList.ToArray(),
            };
            foreach (BlendShape shape in shapes)
            {
                shape.value = 100;
            }

            listOfExpressions.Add(new Expression(name, shapes));
        }
    }

    internal void TriggerExpression(ExpressionName expressionName)
    {
        Expression expression = listOfExpressions.FirstOrDefault((Expression expression) => expression.Name == expressionName);
        AnimatorClipInfo[] clipInfo = controller.animator.GetCurrentAnimatorClipInfo(layerIndex);
        if (controller.animator != null && expression != null)
        {

            controller.animator.CrossFade(expression.Name.ToString(), 0.3f, layerIndex);
        }
    }

    internal void SetExpressionMotions()
    {
        ChildAnimatorState[] states = controller.GetLayerStates(layerIndex);
        foreach (Expression expression in listOfExpressions)
        {
            AnimationClip clip = controller.CreateClip(expression.Name.ToString());

            foreach (BlendShape shape in expression.shapes)
            {
                controller.CreateLinearCurve(clip, shape.Name);
            }
            ChildAnimatorState expressionState = states.FirstOrDefault((ChildAnimatorState state) => state.state.name == expression.Name.ToString());
            if (expressionState.state != null)
            {
                expressionState.state.motion = clip;
            }
        }
    }

}
public class Expression
{
    public ExpressionName Name;
    public BlendShape[] shapes;

    public Expression(ExpressionName name, BlendShape[] shapes)
    {
        Name = name;
        this.shapes = shapes;
    }
}

public enum ExpressionName
{
    Happy,
    Sad,
    Angry,
    Confused,
    Shocked,
}