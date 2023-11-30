using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterBlink : MonoBehaviour
{
    private CharacterAnimationController controller;
    private int layerIndex;
    private readonly string blendName = "Blink";
    internal void Initialize(CharacterAnimationController controller)
    {
        this.controller = controller;
    }
    void Start() {
        SetBlinkMotion();
    }

    internal void SetBlinkMotion()
    {
        print(controller.animator);
        layerIndex = controller.animator.GetLayerIndex("Eyes Layer");
        ChildAnimatorState[] states = controller.GetLayerStates(layerIndex);
        AnimationClip clip = controller.CreateClip(blendName);
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);
        Keyframe[] keys = new Keyframe[4];
        keys[0] = new Keyframe(5, 0);
        keys[1] = new Keyframe(5.2f, 100);
        keys[2] = new Keyframe(5.4f, 100);
        keys[3] = new Keyframe(5.6f, 0);



        int blendShapeIndex = controller.skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(blendName);
        if (blendShapeIndex != -1)
        {
            AnimationCurve curve = new(keys);
            string propertyName = "blendShape." + blendName;
            clip.SetCurve(controller.skinnedMeshRenderer.transform.name, typeof(SkinnedMeshRenderer), propertyName, curve);
        }
        ChildAnimatorState blinkState = states.FirstOrDefault((ChildAnimatorState state) => state.state.name == blendName);
        if (blinkState.state != null)
        {
            blinkState.state.motion = clip;
        }
    }
}
