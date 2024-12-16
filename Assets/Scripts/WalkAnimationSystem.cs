
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[Serializable]
public class WalkTurnAnimationInformation
{
    [SerializeField]
    private AnimationClip _forwardTurnAnimation;
    [SerializeField]
    private AnimationClip _backTurnAnimation;
    [SerializeField]
    private AnimationClip _rightTurnAnimation;
    [SerializeField]
    private AnimationClip _LeftTurnAnimation;

    public AnimationClip ForwardTurnAnimation { get => _forwardTurnAnimation; }
    public AnimationClip BackTurnAnimation { get => _backTurnAnimation; }
    public AnimationClip RightTurnAnimation { get => _rightTurnAnimation; }
    public AnimationClip LeftTurnAnimation { get => _LeftTurnAnimation; }
}

public class WalkAnimationSystem: MonoBehaviour
{
    [SerializeField]
    AnimationCurve _curve;

    private Animator _animator;

    private PlayableGraph _playableGraph;
    private Playable _playable;
    readonly AnimationPlayableOutput _playableOutput;

    void OnDestroy()
    {
        CleanupPlayable();
    }

    public WalkAnimationSystem(Animator animator)
    {
        _animator = animator;
        _playableGraph = PlayableGraph.Create();
        _playableOutput = AnimationPlayableOutput.Create(_playableGraph, name, animator);
        _playableOutput.SetWeight(0f);
    }

    public IEnumerator AnimationPlay(float time, AnimationClip animationClip)
    {
        MyExtensionClass.CheckArgumentNull(animationClip, nameof(animationClip));

        CleanupPlayable();
        SetupNewPlayable(animationClip);

        yield return StartTransition(time, true);
        _animator.playableGraph.Stop();

        float playTime = animationClip.length - (time * 2);

        if (playTime > 0)
        {
            yield return new WaitForSeconds(playTime);
        }

        _animator.playableGraph.Play();
        yield return StartTransition(time, false);

        CleanupPlayable();
    }

    private void SetupNewPlayable(AnimationClip animClip)
    {
        _playable = AnimationClipPlayable.Create(_playableGraph, animClip);
        _playableOutput.SetSourcePlayable(_playable);
        _playableGraph.Play();
    }

    private IEnumerator StartTransition(float durection, bool isIn)
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + durection;

        while (Time.timeSinceLevelLoad < endTime)
        {
            float nowTime = (Time.timeSinceLevelLoad - startTime) / durection;
            if (!isIn)
            {
                nowTime = 1 - nowTime;
            }
            _playableOutput.SetWeight(_curve.Evaluate(nowTime));
            yield return null;
        }
    }

    private void CleanupPlayable()
    {
        if (_playableGraph.IsValid()) 
        {
            _playableGraph.Destroy(); //–¾Ž¦“I‚ÉDestroy
            _playableGraph = default;
        }
        Debug.Log("Playable”j‰ó");
    }
}
