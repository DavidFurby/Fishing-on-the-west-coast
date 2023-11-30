using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterBlink))]
[RequireComponent(typeof(CharacterExpression))]
[RequireComponent(typeof(CharacterGesture))]
public class CharacterAnimationController : MonoBehaviour
{
    internal CharacterExpression expression;
    internal CharacterGesture gesture;
    internal CharacterBlink blink;
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
        expression = GetComponent<CharacterExpression>();
        gesture = GetComponent<CharacterGesture>();
        blink = GetComponent<CharacterBlink>();
        expression.Initialize(this);
        gesture.Initialize(this);
        blink.Initialize(this);

    }
    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;
        SetBlendShapes();
        expression.SetExpressions();
        gesture.SetGestures();
        blink.SetBlinkMotion();
        expression.SetExpressionMotions();
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