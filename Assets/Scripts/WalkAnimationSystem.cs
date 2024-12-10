using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class WalkAnimationInformation
{
    [SerializeField]
    private AnimationClip _forwardWalkAnimation;

    [SerializeField]
    private AnimationClip _backWalkAnimation;

    [SerializeField]
    private AnimationClip _rightWalkAnimation;

    [SerializeField]
    private AnimationClip _leftWalkAnimation;

    public AnimationClip ForwardWalkAnimation { get => _forwardWalkAnimation; }
    public AnimationClip BackWalkAnimation { get => _backWalkAnimation; }
    public AnimationClip RightWalkAnimation { get => _rightWalkAnimation; }
    public AnimationClip LeftWalkAnimation { get => _leftWalkAnimation; }

    public WalkAnimationInformation(AnimationClip fwAnim, AnimationClip backAnim, AnimationClip rightAnim ,AnimationClip leftAnim)
    {
        _forwardWalkAnimation = fwAnim;
        _backWalkAnimation = backAnim;
        _rightWalkAnimation = rightAnim;
        _leftWalkAnimation = leftAnim;
    }
}

public class WalkAnimationSystem : MonoBehaviour
{
    PlayableGraph _playableGraph;
    readonly AnimationMixerPlayable topLevelMixer;
    readonly AnimationMixerPlayable locomotionMixer;

    AnimationClipPlayable oneShotPlayable;

    public WalkAnimationSystem(Animator animator, WalkAnimationInformation walkAnimation)
    {
        _playableGraph = PlayableGraph.Create("WalkAnimationSystem");

        AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", animator);

        topLevelMixer = AnimationMixerPlayable.Create(_playableGraph, 2);
        playableOutput.SetSourcePlayable(topLevelMixer);

        locomotionMixer = AnimationMixerPlayable.Create(_playableGraph, 2);
        topLevelMixer.ConnectInput(0, locomotionMixer, 0);
        _playableGraph.GetRootPlayable(0).SetInputWeight(0, 1f);

        AnimationClipPlayable fwWalkPlayable = AnimationClipPlayable.Create(_playableGraph, walkAnimation.ForwardWalkAnimation);
        AnimationClipPlayable backWalkPlayable = AnimationClipPlayable.Create(_playableGraph, walkAnimation.BackWalkAnimation);
        AnimationClipPlayable rightWalkPlayable = AnimationClipPlayable.Create(_playableGraph, walkAnimation.RightWalkAnimation);
        AnimationClipPlayable leftWalkPlayable = AnimationClipPlayable.Create(_playableGraph, walkAnimation.LeftWalkAnimation);

        fwWalkPlayable.GetAnimationClip().wrapMode = WrapMode.Loop;
        backWalkPlayable.GetAnimationClip().wrapMode = WrapMode.Loop;
        rightWalkPlayable.GetAnimationClip().wrapMode = WrapMode.Loop;
        leftWalkPlayable.GetAnimationClip().wrapMode = WrapMode.Loop;

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
