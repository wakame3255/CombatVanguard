using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using R3;

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

public class WalkAnimationSystem : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curve;

    private Animator _animator;
    private PlayableGraph _playableGraph;
    private Playable _playable;
    private AnimationPlayableOutput _playableOutput;

    private ReactiveProperty<bool> _reactivePropertyIsAnimation = new ReactiveProperty<bool>(false);

    public ReactiveProperty<bool> ReactivePropertyIsAnimation { get => _reactivePropertyIsAnimation; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        InitializePlayableGraph();
    }

    private void OnDestroy()
    {
        CleanupPlayable();
    }

    private void OnValidate()
    {
        if (_curve == null)
        {
            _curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        }
    }

    private void InitializePlayableGraph()
    {
        if (!_playableGraph.IsValid())
        {
            _playableGraph = PlayableGraph.Create();
            _playableOutput = AnimationPlayableOutput.Create(_playableGraph, name, _animator);
            _playableOutput.SetWeight(0f);
        }
    }

    public IEnumerator AnimationPlay(float time, AnimationClip animationClip)
    {
        MyExtensionClass.CheckArgumentNull(animationClip, nameof(animationClip));

        Debug.Log("�A�j���[�V�����J�n");
        _reactivePropertyIsAnimation.Value = true;

        // �O���t�̏��������m���ɍs��
        InitializePlayableGraph();

        // ������Playable���N���[���A�b�v
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }

        // �V����Playable�̐ݒ�
        SetupNewPlayable(animationClip);

        yield return StartTransition(time, true);
        _playableGraph.Stop();

        float playTime = animationClip.length - (time * 2);

        if (playTime > 0)
        {
            yield return new WaitForSeconds(playTime);
        }

        _playableGraph.Play();
        yield return StartTransition(time, false);

        // Playable�̃N���[���A�b�v�݂̂��s���A�O���t�͈ێ�
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }

        Debug.Log("�A�j���[�V�����I��");
        _reactivePropertyIsAnimation.Value = false;
    }

    private void SetupNewPlayable(AnimationClip animClip)
    {
        _playable = AnimationClipPlayable.Create(_playableGraph, animClip);

        _playableOutput.SetSourcePlayable(_playable);
        _playableGraph.Play();
    }

    private IEnumerator StartTransition(float duration, bool isIn)
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + duration;

        while (Time.timeSinceLevelLoad < endTime)
        {
            float nowTime = (Time.timeSinceLevelLoad - startTime) / duration;
            if (!isIn)
            {
                nowTime = 1 - nowTime;
            }
            _playableOutput.SetWeight(_curve.Evaluate(nowTime));
            yield return null;
        }

        // �ŏI�I�ȏd�݂��m���ɐݒ�
        _playableOutput.SetWeight(isIn ? 1f : 0f);
    }

    private void CleanupPlayable()
    {
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }

        if (_playableGraph.IsValid())
        {
            _playableGraph.Destroy();
            Debug.Log("PlayableGraph��j�����܂���");
        }
    }

#if UNITY_EDITOR
    private void OnDisable()
    {
        // �G�f�B�^�ł̎��s��~���ɂ����\�[�X�����
        CleanupPlayable();
    }
#endif
}