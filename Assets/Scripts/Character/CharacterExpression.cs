using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterExpression : MonoBehaviour
{
    internal SkinnedMeshRenderer skinnedMeshRenderer;
    private CharacterManager manager;
    int blendShapeCount;
    public List<BlendShape> ShapeList { get; set; } = new List<BlendShape>();
    public List<Expression> listOfExpressions = new();
    private int layerIndex;
    internal void Initialize(CharacterManager manager)
    {
        this.manager = manager;
    }

    void Start()
    {
        layerIndex = manager.animations.animator.GetLayerIndex("Expression Layer");
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;
        SetBlendShapes();
        SetExpressions();
        manager.animations.SetGestures();
        manager.blink.SetBlinkMotion();
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
        AnimatorClipInfo[] clipInfo = manager.animations.animator.GetCurrentAnimatorClipInfo(layerIndex);
        if (manager.animations.animator != null && expression != null)
        {

            manager.animations.animator.CrossFade(expression.Name.ToString(), 0.3f, layerIndex);
        }
    }

    private void SetExpressionMotions()
    {
        ChildAnimatorState[] states = GetLayerStates(layerIndex);
        foreach (Expression expression in listOfExpressions)
        {
            AnimationClip clip = CreateClip(expression.Name.ToString());

            foreach (BlendShape shape in expression.shapes)
            {
                CreateLinearCurve(clip, shape.Name);
            }
            ChildAnimatorState expressionState = states.FirstOrDefault((ChildAnimatorState state) => state.state.name == expression.Name.ToString());
            if (expressionState.state != null)
            {
                expressionState.state.motion = clip;
            }
        }
    }

    internal AnimationClip CreateClip(string clipName)
    {
        AnimationClip clip = new()
        {
            name = clipName,
        };

        return clip;

    }

    internal void CreateLinearCurve(AnimationClip clip, string shapeName)
    {
        int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(shapeName);
        if (blendShapeIndex != -1)
        {
            AnimationCurve curve = AnimationCurve.Linear(0f, 0, 1f, 100);
            string propertyName = "blendShape." + shapeName;
            clip.SetCurve(skinnedMeshRenderer.transform.name, typeof(SkinnedMeshRenderer), propertyName, curve);
        }
    }

    private string GetRelativePath(Transform current)
    {
        if (current.parent == null)
            return "/" + current.name;
        else
            return GetRelativePath(current.parent) + "/" + current.name;
    }

    internal ChildAnimatorState[] GetLayerStates(int index)
    {
        AnimatorController ac = manager.animations.animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer layer = ac.layers[index];
        AnimatorStateMachine stateMachine = layer.stateMachine;
        ChildAnimatorState[] states = stateMachine.states;
        return states;
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