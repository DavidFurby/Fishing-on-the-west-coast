using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterExpression : MonoBehaviour
{
    private CharacterAnimationController controller;
    public List<BlendShape> ShapeList { get; set; } = new List<BlendShape>();
    public List<Expression> listOfExpressions = new();
    private int layerIndex;
    internal void Initialize(CharacterAnimationController controller)
    {
        this.controller = controller;
    }

    void Start()
    {
        layerIndex = controller.gesture.animator.GetLayerIndex("Expression Layer");
    }


    internal void SetExpressions()
    {
        foreach (ExpressionName name in System.Enum.GetValues(typeof(ExpressionName)))
        {
            BlendShape[] shapes = name switch
            {
                ExpressionName.Happy => ShapeList.Where((BlendShape shape) => shape.Name == "Blink Happy" || shape.Name == "Mouth Open").ToArray(),
                ExpressionName.Shocked => ShapeList.Where((BlendShape shape) => shape.Name == "Mouth Open").ToArray(),
                ExpressionName.Sad => ShapeList.Where((BlendShape shape) => shape.Name == "Eyes Sad" || shape.Name == "Mouth Sad").ToArray(),
                _ => ShapeList.ToArray(),
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
        AnimatorClipInfo[] clipInfo = controller.gesture.animator.GetCurrentAnimatorClipInfo(layerIndex);
        if (controller.gesture.animator != null && expression != null)
        {

            controller.gesture.animator.CrossFade(expression.Name.ToString(), 0.3f, layerIndex);
        }
    }

    internal void SetExpressionMotions()
    {
        ChildAnimatorState[] states = GetLayerStates(layerIndex);
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


    internal ChildAnimatorState[] GetLayerStates(int index)
    {
        AnimatorController ac = controller.gesture.animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer layer = ac.layers[index];
        AnimatorStateMachine stateMachine = layer.stateMachine;
        ChildAnimatorState[] states = stateMachine.states;
        return states;
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