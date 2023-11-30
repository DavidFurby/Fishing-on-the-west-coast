using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterBlink : MonoBehaviour
{
    private CharacterAnimationController controller;
    private int layerIndex;
    internal void Initialize(CharacterAnimationController controller)
    {
        this.controller = controller;
    }

    internal void SetBlinkMotion()
    {
        layerIndex = controller.gesture.animator.GetLayerIndex("Eyes Layer");
        ChildAnimatorState[] states = controller.expression.GetLayerStates(layerIndex);
        AnimationClip clip = controller.CreateClip("Blink");
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);
        Keyframe[] keys = new Keyframe[4];
        keys[0] = new Keyframe(5, 0);
        keys[1] = new Keyframe(5.2f, 100);
        keys[2] = new Keyframe(5.4f, 100);
        keys[3] = new Keyframe(5.6f, 0);



        int blendShapeIndex = controller.skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("Blink");
        if (blendShapeIndex != -1)
        {
            AnimationCurve curve = new(keys);
            string propertyName = "blendShape." + "Blink";
            clip.SetCurve(controller.skinnedMeshRenderer.transform.name, typeof(SkinnedMeshRenderer), propertyName, curve);
        }
        ChildAnimatorState blinkState = states.FirstOrDefault((ChildAnimatorState state) => state.state.name == "Blink");
        if (blinkState.state != null)
        {
            blinkState.state.motion = clip;
        }
    }
}
