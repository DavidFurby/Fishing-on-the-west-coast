using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterBlink : MonoBehaviour
{
    private CharacterManager manager;
    private int layerIndex;

    internal void Initialize(CharacterManager manager)
    {
        this.manager = manager;
    }
 
    internal void SetBlinkMotion()
    {
        layerIndex = manager.animations.animator.GetLayerIndex("Eyes Layer");

        ChildAnimatorState[] states = manager.expression.GetLayerStates(layerIndex);
        AnimationClip clip = new()
        {
            name = "Blink"
        };
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        int blendShapeIndex = manager.expression.skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("Blink");
        if (blendShapeIndex != -1)
        {
            AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 100f);
            string propertyName = "blendShape." + "Blink";
            clip.SetCurve(manager.expression.skinnedMeshRenderer.transform.name, typeof(SkinnedMeshRenderer), propertyName, curve);
        }
        ChildAnimatorState blinkState = states.FirstOrDefault((ChildAnimatorState state) => state.state.name == "Blink");
        if (blinkState.state != null)
        {
            blinkState.state.motion = clip;
        }
    }
}
