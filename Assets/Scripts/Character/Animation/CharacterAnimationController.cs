using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(CharacterBlink))]
[RequireComponent(typeof(CharacterExpression))]
[RequireComponent(typeof(CharacterGesture))]
[RequireComponent(typeof(CharacterTalk))]
public class CharacterAnimationController : MonoBehaviour
{
    internal CharacterExpression expression;
    internal CharacterGesture gesture;
    internal CharacterBlink blink;
    internal CharacterTalk talk;
    internal CharacterManager manager;
    internal SkinnedMeshRenderer skinnedMeshRenderer;
    internal Animator animator;
    public List<BlendShape> ShapeList { get; set; } = new List<BlendShape>();


    int blendShapeCount;

    public void Initialize(CharacterManager manager)
    {
        this.manager = manager;
    }

    void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;
        animator = GetComponentInChildren<Animator>();
        SetBlendShapes();

    }
    void OnEnable()
    {
        expression = GetComponent<CharacterExpression>();
        gesture = GetComponent<CharacterGesture>();
        blink = GetComponent<CharacterBlink>();
        talk = GetComponent<CharacterTalk>();
        expression.Initialize(this);
        gesture.Initialize(this);
        blink.Initialize(this);
        talk.Initialize(this);
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
        AnimatorController ac = animator.runtimeAnimatorController as AnimatorController;
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