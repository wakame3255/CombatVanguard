
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�g�����ăA�j���[�V�����𔻒f����N���X
/// </summary>
public class CharacterAnimationJudge
{
    private Dictionary<Type, MatchTargetAnimationData> _insertAnimationDictionary;

    private Dictionary<Type, String> _loopAnimationDictionary;

    private CharacterAnimation _characterAnimation;

    public CharacterAnimationJudge(CharacterAnimation characterAnimation)
    {
        _insertAnimationDictionary = new Dictionary<Type, MatchTargetAnimationData>();

        _characterAnimation = characterAnimation;

        SetAnimation(characterAnimation);
    }

    /// <summary>
    /// �A�j���[�V�����̔��f��S�����\�b�h
    /// </summary>
    /// <param name="stateData"></param>
    public void JudgePlayAnimation(StateJudgeBase stateData)
    {
        if (_insertAnimationDictionary.TryGetValue(stateData.GetType(), out MatchTargetAnimationData animationData))
        {
            _characterAnimation.DoAnimation(animationData);
            return;
        }

        //Debug.LogWarning($"�A�j���[�V������������܂���: {stateData.GetType()}");

    }

    /// <summary>
    /// �A�j���[�V�������Z�b�g����
    /// </summary>
    /// <param name="characterAnimation">�A�j���[�V�������</param>
    private void SetAnimation(CharacterAnimation characterAnimation)
    {
        // �������݃A�j���[�V�����Q
        SetInsertAnimationDictionary<AttackStateJudge>(characterAnimation.AnimationData.AttackAnimation.JabAnimation);
        SetInsertAnimationDictionary<DownStateJudge>(characterAnimation.AnimationData.AttackAnimation.HitAnimation);
        SetInsertAnimationDictionary<GuardHitStateJudge>(characterAnimation.AnimationData.AttackAnimation.GuardHitAnimation);
        SetInsertAnimationDictionary<AvoidanceStateJudge>(characterAnimation.AnimationData.InterruptionAnimation.AvoidanceAnimation);
        SetInsertAnimationDictionary<HitParryStateJudge>(characterAnimation.AnimationData.InterruptionAnimation.HitParryAnimation);
        SetInsertAnimationDictionary<ParryStateJudge>(characterAnimation.AnimationData.InterruptionAnimation.ParryAnimation);

        //Loop�A�j���[�V�����Q
        //SetLoopAnimationDictionary<WalkStateJudge>();
        DebugUtility.Log("kokokara");
    }

    /// <summary>
    /// �A�j���[�V�����f�[�^���Z�b�g����
    /// </summary>
    /// <typeparam name="T">StateBase�̔h���N���X</typeparam>
    /// <param name="animationData">�A�j���[�V����</param>
    private void SetInsertAnimationDictionary<T>(MatchTargetAnimationData animationData) where T : StateJudgeBase
    {
        _insertAnimationDictionary.Add(typeof(T), animationData);
    }

    private void SetLoopAnimationDictionary<T>(string animationData) where T : StateJudgeBase
    {
        _loopAnimationDictionary.Add(typeof(T), animationData);


    }
}
