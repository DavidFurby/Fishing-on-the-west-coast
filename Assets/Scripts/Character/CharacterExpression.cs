using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEditor;

public class CharacterExpression : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    private CharacterManager manager;
    int blendShapeCount;
    public List<BlendShape> ShapeList { get; set; } = new List<BlendShape>();
    public List<Expression> listOfExpressions = new();
    private Expression activeExpression;
    internal void Initialize(CharacterManager manager)
    {
        this.manager = manager;
    }

    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;
        SetBlendShapes();
        SetExpressions();
        manager.animations.SetGestures();
        SetExpressionMotions();
    }

    private void SetBlendShapes()
    {
        for (int i = 0; i < blendShapeCount; i++)
        {
            string name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i);
            float blendShapeValue = skinnedMeshRenderer.GetBlendShapeWeight(i);
            ShapeList.Add(new BlendShape(name, blendShapeValue));
        }
    }
    private void SetExpressions()
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
            listOfExpressions.Add(new Expression(name, shapes));
        }
    }
    private void UpdateExpressionValues(int index, float newValue)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(index, newValue);
        ShapeList[index].value = newValue;
    }
    internal void TriggerExpression(ExpressionName expressionName, bool active)
    {
        Expression expression = listOfExpressions.FirstOrDefault((Expression expression) => expression.Name == expressionName);

        if (activeExpression == null && manager.animations.animator != null && expression != null)
        {
            print(expression.shapes[0].value);
            manager.animations.animator.SetBool(expressionName.ToString().ToLower(), active);
            activeExpression = expression;
        }
    }
    private void SetExpressionMotions()
    {
        ChildAnimatorState[] states = GetExpressionStates();
        foreach (Expression expression in listOfExpressions)
        {
            AnimationClip clip = new()
            {
                name = expression.Name.ToString()
            };

            foreach (BlendShape shape in expression.shapes)
            {
                AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, shape.value);
                string propertyName = "blendShape." + shape.Name;
                clip.SetCurve("", typeof(SkinnedMeshRenderer), propertyName, curve);
            }

            ChildAnimatorState expressionState = states.FirstOrDefault((ChildAnimatorState state) => state.state.name == expression.Name.ToString());
            if (expressionState.state != null)
            {
                print(expressionState.state.name);
                expressionState.state.motion = clip;

            }
        }

    }

    private ChildAnimatorState[] GetExpressionStates()
    {
        AnimatorController ac = manager.animations.animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer layer = ac.layers[1];
        AnimatorStateMachine stateMachine = layer.stateMachine;
        ChildAnimatorState[] states = stateMachine.states;
        return states;
    }

    internal void MouthFlaps()
    {
        BlendShape mouthMovement = ShapeList[0];
        float newValue = mouthMovement.value <= 50 ? 100 : 0;
        UpdateExpressionValues(0, newValue);
    }
}
public class BlendShape
{
    public string Name;
    public float value;

    public BlendShape(string name, float value)
    {
        Name = name;
        this.value = value;
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