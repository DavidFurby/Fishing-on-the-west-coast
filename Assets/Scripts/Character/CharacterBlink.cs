using System.Collections;
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
        AnimationClip clip = manager.expression.CreateClip("Blink");
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);
        Keyframe[] keys = new Keyframe[4];
        keys[0] = new Keyframe(5, 0);
        keys[1] = new Keyframe(5.2f, 100);
        keys[2] = new Keyframe(5.4f, 100);
        keys[3] = new Keyframe(5.6f, 0);



        int blendShapeIndex = manager.expression.skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("Blink");
        if (blendShapeIndex != -1)
        {
            AnimationCurve curve = new(keys);
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
