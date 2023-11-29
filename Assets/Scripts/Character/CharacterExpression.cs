using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

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
    void Update() {
        CheckIfExpressionIsDone();
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
            foreach (BlendShape shape in shapes)
            {
                shape.value = 100;
            }

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
            manager.animations.animator.CrossFade(expression.Name.ToString(), 0.3f, 1);
            activeExpression = expression;
        }
    }
    public void CheckIfExpressionIsDone()
    {
        if (activeExpression != null)
        {
            int index = manager.animations.animator.GetLayerIndex("Expression Layer");
            AnimatorStateInfo stateInfo = manager.animations.animator.GetCurrentAnimatorStateInfo(index);
            if (stateInfo.IsName(activeExpression.Name.ToString()) && stateInfo.normalizedTime > 1)
            {
                print("expression done");
                manager.animations.animator.CrossFade("Default", 0.3f, index);
                activeExpression = null;
            }
        }
    }

    private void SetExpressionMotions()
    {
        ChildAnimatorState[] states = GetExpressionStates();
        foreach (Expression expression in listOfExpressions)
        {
            AnimationClip clip = new()
            {
                name = expression.Name.ToString(),
                wrapMode = WrapMode.Once

            };

            foreach (BlendShape shape in expression.shapes)
            {
                int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(shape.Name);
                if (blendShapeIndex != -1)
                {
                    AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 100f);
                    string propertyName = "blendShape." + shape.Name;
                    clip.SetCurve(skinnedMeshRenderer.transform.name, typeof(SkinnedMeshRenderer), propertyName, curve);
                }
            }

            ChildAnimatorState expressionState = states.FirstOrDefault((ChildAnimatorState state) => state.state.name == expression.Name.ToString());
            if (expressionState.state != null)
            {
                expressionState.state.motion = clip;
            }
        }
    }
    private string GetRelativePath(Transform current)
    {
        if (current.parent == null)
            return "/" + current.name;
        else
            return GetRelativePath(current.parent) + "/" + current.name;
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