using System.Collections.Generic;
using UnityEngine;

public class CharacterExpression : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    CharacterController controller;

    int blendShapeCount;
    public List<Expression> ExpressionList { get; set; } = new List<Expression>();

    internal void Initialize(CharacterController controller)
    {
        this.controller = controller;
    }

    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;
        SetExpressionValues();
        MouthFlaps();
    }

    private void SetExpressionValues()
    {
        for (int i = 0; i < blendShapeCount; i++)
        {
            string name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i);
            float blendShapeValue = skinnedMeshRenderer.GetBlendShapeWeight(i);
            ExpressionList.Add(new Expression(name, blendShapeValue));
        }
    }
    private void UpdateExpressionValues(int index, float newValue)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(index, newValue);
        print(skinnedMeshRenderer.GetBlendShapeWeight(index));
        ExpressionList[index].value = newValue;
    }

    internal void MouthFlaps()
    {
        Expression mouthMovement = ExpressionList[0];
        float newValue = mouthMovement.value <= 50 ? 100 : 0;
        print(mouthMovement.Name);
        print(newValue);
        UpdateExpressionValues(0, newValue);
    }
}
public class Expression
{
    public string Name;
    public float value;

    public Expression(string name, float value)
    {
        Name = name;
        this.value = value;
    }
}