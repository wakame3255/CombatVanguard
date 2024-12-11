using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[Serializable]
public class WalkAnimationInformation
{
    [SerializeField]
    private AnimationClip _idleAnimation;

    [SerializeField]
    private AnimationClip _forwardWalkAnimation;

    [SerializeField]
    private AnimationClip _backWalkAnimation;

    [SerializeField]
    private AnimationClip _rightWalkAnimation;

    [SerializeField]
    private AnimationClip _leftWalkAnimation;

    public AnimationClip IdleAnimation { get => _idleAnimation; }
    public AnimationClip ForwardWalkAnimation { get => _forwardWalkAnimation; }
    public AnimationClip BackWalkAnimation { get => _backWalkAnimation; }
    public AnimationClip RightWalkAnimation { get => _rightWalkAnimation; }
    public AnimationClip LeftWalkAnimation { get => _leftWalkAnimation; }
}

public class WalkAnimationSystem
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

        locomotionMixer = AnimationMixerPlayable.Create(_playableGraph, 5);
        topLevelMixer.ConnectInput(0, locomotionMixer, 0);
        _playableGraph.GetRootPlayable(0).SetInputWeight(0, 1f);

        AnimationClipPlayable idlePlayable = AnimationClipPlayable.Create(_playableGraph, walkAnimation.IdleAnimation);
        AnimationClipPlayable fwWalkPlayable = AnimationClipPlayable.Create(_playableGraph, walkAnimation.ForwardWalkAnimation);
        AnimationClipPlayable backWalkPlayable = AnimationClipPlayable.Create(_playableGraph, walkAnimation.BackWalkAnimation);
        AnimationClipPlayable rightWalkPlayable = AnimationClipPlayable.Create(_playableGraph, walkAnimation.RightWalkAnimation);
        AnimationClipPlayable leftWalkPlayable = AnimationClipPlayable.Create(_playableGraph, walkAnimation.LeftWalkAnimation);

        idlePlayable.GetAnimationClip().wrapMode = WrapMode.Loop;
        fwWalkPlayable.GetAnimationClip().wrapMode = WrapMode.Loop;
        backWalkPlayable.GetAnimationClip().wrapMode = WrapMode.Loop;
        rightWalkPlayable.GetAnimationClip().wrapMode = WrapMode.Loop;
        leftWalkPlayable.GetAnimationClip().wrapMode = WrapMode.Loop;

        locomotionMixer.ConnectInput(0, idlePlayable, 0);
        locomotionMixer.ConnectInput(1, fwWalkPlayable, 0);
        locomotionMixer.ConnectInput(2, backWalkPlayable, 0);
        locomotionMixer.ConnectInput(3, rightWalkPlayable, 0);
        locomotionMixer.ConnectInput(4, leftWalkPlayable, 0);

        _playableGraph.Play();
    }


    public void UpdateLocomotion(Vector2 InputXY, float maxSpeed)
    {     
         float idleWeight = Mathf.InverseLerp(0f, 1f, InputXY.sqrMagnitude);
        // 正負の値に対応したウェイト計算
        float forwardWeight = Mathf.Max(0, InputXY.y) / maxSpeed;
        float backWeight = Mathf.Max(0, -InputXY.y) / maxSpeed;
        float rightWeight = Mathf.Max(0, InputXY.x) / maxSpeed;
        float leftWeight = Mathf.Max(0, -InputXY.x) / maxSpeed;

        locomotionMixer.SetInputWeight(0, 1 - idleWeight);
        locomotionMixer.SetInputWeight(1, forwardWeight);
        locomotionMixer.SetInputWeight(2, backWeight);
        locomotionMixer.SetInputWeight(3, rightWeight);
        locomotionMixer.SetInputWeight(4, leftWeight);
    }

    public void Destroy()
    {
        if (_playableGraph.IsValid())
        {
            _playableGraph.Destroy();
        }
    }
}
