using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimationSystem : MonoBehaviour
{
    PlayableGraph _playableGraph;
    readonly AnimationMixerPlayable topLevelMixer;
    readonly AnimationMixerPlayable locomotionMixer;

    AnimationClipPlayable oneShotPlayable;

    public AnimationSystem(Animator animator, AnimationClip idleClip, AnimationClip walkClip)
    {
        _playableGraph = PlayableGraph.Create("AnimationSystem");

        AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", animator);

        topLevelMixer = AnimationMixerPlayable.Create(_playableGraph, 2);
        playableOutput.SetSourcePlayable(topLevelMixer);

        locomotionMixer = AnimationMixerPlayable.Create(_playableGraph, 2);
        topLevelMixer.ConnectInput(0, locomotionMixer, 0);
        _playableGraph.GetRootPlayable(0).SetInputWeight(0, 1f);

        AnimationClipPlayable idlePlayable = AnimationClipPlayable.Create(_playableGraph, idleClip);
        AnimationClipPlayable walkPlayable = AnimationClipPlayable.Create(_playableGraph, walkClip);

        idlePlayable.GetAnimationClip().wrapMode = WrapMode.Loop;
        walkPlayable.GetAnimationClip().wrapMode = WrapMode.Loop;

        locomotionMixer.ConnectInput(0, idlePlayable, 0);
        locomotionMixer.ConnectInput(1, walkPlayable, 0);

        _playableGraph.Play();
    }


    public void UpdateLocomotion(Vector3 velocity, float maxSpeed)
    {
        float weight = Mathf.InverseLerp(0f, maxSpeed, velocity.magnitude);
        locomotionMixer.SetInputWeight(0, 1 - weight);
        locomotionMixer.SetInputWeight(1, weight);
    }

    public void Destroy()
    {
        if (_playableGraph.IsValid())
        {
            _playableGraph.Destroy();
        }
    }
}
