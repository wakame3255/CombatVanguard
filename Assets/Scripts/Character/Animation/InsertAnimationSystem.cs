using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using R3;

[Serializable]
public class AttackAnimationInformation
{
    [SerializeField]
    private MatchTargetAnimationData _jabAnimation;

    [SerializeField]
    private MatchTargetAnimationData _hitAnimation;

    public MatchTargetAnimationData JabAnimation { get => _jabAnimation; }
    public MatchTargetAnimationData HitAnimation { get => _hitAnimation; }
}

[Serializable]
public class WalkTurnAnimationInformation
{
    [SerializeField]
    private MatchTargetAnimationData _forwardTurnAnimation;

    [SerializeField]
    private MatchTargetAnimationData _avoidanceAnimation;

    public MatchTargetAnimationData ForwardTurnAnimation { get => _forwardTurnAnimation; }
    public MatchTargetAnimationData AvoidanceAnimation { get => _avoidanceAnimation; }
}

public class InsertAnimationSystem : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curve;

    [SerializeField]
    private Transform _targetPos;

    private TargetMatchMove _targetMatchMove;
    private Animator _animator;
    private PlayableGraph _playableGraph;
    private Playable _playable;
    private AnimationPlayableOutput _playableOutput;

    private ReactiveProperty<bool> _reactivePropertyIsAnimation = new ReactiveProperty<bool>(false);

    public ReactiveProperty<bool> ReactivePropertyIsAnimation { get => _reactivePropertyIsAnimation; }

    private void Awake()
    {
        // TargetMatchMove�R���|�[�l���g��ǉ�
        _targetMatchMove = gameObject.AddComponent<TargetMatchMove>();
        // Animator�R���|�[�l���g���擾
        _animator = GetComponent<Animator>();
        // Playable�O���t��������
        InitializePlayableGraph();
    }

    private void OnDestroy()
    {
        // Playable���\�[�X���N���[���A�b�v
        CleanupPlayable();
    }

    private void OnValidate()
    {
        // �J�[�u���ݒ肳��Ă��Ȃ��ꍇ�A�f�t�H���g�̃J�[�u�𐶐�
        if (_curve == null)
        {
            _curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        }
    }

    private void InitializePlayableGraph()
    {
        // Playable�O���t�������ȏꍇ�ɐV�K�쐬
        if (!_playableGraph.IsValid())
        {
            _playableGraph = PlayableGraph.Create();
            _playableOutput = AnimationPlayableOutput.Create(_playableGraph, name, _animator);
            _playableOutput.SetWeight(0f);
        }
    }

    public IEnumerator AnimationPlay(MatchTargetAnimationData animationClip)
    {
        // �A�j���[�V�����N���b�v��null�`�F�b�N
        MyExtensionClass.CheckArgumentNull(animationClip, nameof(animationClip));

        // �A�j���[�V�������̏�Ԃ�true�ɐݒ�
        _reactivePropertyIsAnimation.Value = true;

        // Playable�O���t�̏��������m���ɍs��
        InitializePlayableGraph();

        // ������Playable���N���[���A�b�v
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }

        // �V����Playable��ݒ�
        SetupNewPlayable(animationClip.AnimationClip);
        // �^�[�Q�b�g�}�b�`���[�V�����̃f�[�^��ݒ�
        _targetMatchMove.SetMatchTargetAnimationData(animationClip, _targetPos);

        // �g�����W�V�����J�n��ҋ@
        yield return StartTransition(animationClip.AnimationClip.length / 2f, true, animationClip);

        // Playable�O���t���~
        _playableGraph.Stop();

        float playTime = animationClip.AnimationClip.length - (animationClip.AnimationClip.length);

        // �c��̍Đ����Ԃ�����Αҋ@
        if (playTime > 0)
        {
            yield return new WaitForSeconds(playTime);
        }

        // Playable�O���t���Đ�
        _playableGraph.Play();

        // �g�����W�V�����I����ҋ@
        yield return EndTransition(animationClip.AnimationClip.length / 2f, false);

        // Playable�̃N���[���A�b�v���s���A�O���t�͈ێ�
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }
        // �A�j���[�V�������̏�Ԃ�false�ɐݒ�
        _reactivePropertyIsAnimation.Value = false;
    }

    /// <summary>
    /// ���荞�܂���A�j���[�V�����̐������\�b�h
    /// </summary>
    /// <param name="animClip">���荞�܂������A�j���N���b�v</param>
    private void SetupNewPlayable(AnimationClip animClip)
    {
        // ���荞�݃A�j����Playable�Ƃ��Đ���
        _playable = AnimationClipPlayable.Create(_playableGraph, animClip);

        // Playable�o�͂ɃZ�b�g
        _playableOutput.SetSourcePlayable(_playable);
        // Playable�O���t���Đ�
        _playableGraph.Play();
    }

    private IEnumerator StartTransition(float duration, bool isIn, MatchTargetAnimationData animationData)
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + duration;

        // �g�����W�V�������Ԓ��A�J�[�u�Ɋ�Â��ăE�F�C�g�𒲐�
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

        // �ŏI�I�ȃE�F�C�g���m���ɐݒ�
        _playableOutput.SetWeight(isIn ? 1f : 0f);
    }

    private IEnumerator EndTransition(float duration, bool isIn)
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + duration;

        // �g�����W�V�������Ԓ��A�J�[�u�Ɋ�Â��ăE�F�C�g�𒲐�
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

        // �ŏI�I�ȃE�F�C�g���m���ɐݒ�
        _playableOutput.SetWeight(isIn ? 1f : 0f);
    }

    private void CleanupPlayable()
    {
        // Playable���L���ȏꍇ�͔j��
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }

        // Playable�O���t���L���ȏꍇ�͔j��
        if (_playableGraph.IsValid())
        {
            _playableGraph.Destroy();
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
