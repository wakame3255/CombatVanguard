using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class InsertAnimationSystemPlus : IDisposable
{
    private Animator _animator;

    private PlayableGraph _playableGraph;

    private AnimationClip _currentAnimationClip;

    private AnimationClipPlayable _clipPlayable;

    public InsertAnimationSystemPlus(Animator animator)
    {
        _animator = animator;
    }

    /// <summary>
    /// �v�����Ԃ�O���t�̃��[�N��h��
    /// </summary>
    public void Dispose()
    {
        DebugUtility.Log("�v���C�A�u���̔j��");
        //�v���C�A�u���O���t�������ꂸ�c���Ă���̂�
        if (_playableGraph.IsValid())
        {
            //�v���C�A�u���O���t��j��
            _playableGraph.Destroy();
            //�v���C�A�u���O���t��������
            _playableGraph = default;
        }

        _animator = null;
    }

    public void InsertAnimation(AnimationClip animationClip)
    {
        //�����A�j���[�V�����������ꍇ�̊����߂�
        if (_currentAnimationClip == animationClip)
        {
            //�v�����Ԃ�API�̃��[�J���^�C���̃Z�b�g
            _clipPlayable.SetTime(0);
            //�A�j���[�V�������������Ă��Ȃ��t���O�𗧂Ă�
            _clipPlayable.SetDone(false);
            return;
        }

        //���̃O���t���폜
        Stop();

       AnimationClipPlayable clipPlayable = AnimationPlayableUtilities.PlayClip(_animator, animationClip, out _playableGraph);

        clipPlayable.SetTime(0);
        clipPlayable.SetDuration(animationClip.length);
        clipPlayable.SetDone(false);
    }

    /// <summary>
    /// �Đ����̃A�j���[�V�������~������B
    /// </summary>
    public void Stop()
    {
        if (_clipPlayable.IsValid())
        {
            //�v���C�A�u���O���t��j��
            _playableGraph.Destroy();
            //�v���C�A�u���O���t��������
            _playableGraph = default;
        }

        _currentAnimationClip = null;
    }
}
