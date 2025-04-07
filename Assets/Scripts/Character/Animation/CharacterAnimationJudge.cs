
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートを見てアニメーションを判断するクラス
/// </summary>
public class CharacterAnimationJudge
{
    private Dictionary<Type, MatchTargetAnimationData> _insertAnimationDictionary;

    private Dictionary<Type, String> _loopAnimationDictionary;

    private CharacterAnimation _characterAnimation;

    public CharacterAnimationJudge(CharacterAnimation characterAnimation)
    {
        _insertAnimationDictionary = new Dictionary<Type, MatchTargetAnimationData>();
        _loopAnimationDictionary = new Dictionary<Type, string>();

        _characterAnimation = characterAnimation;

        SetAnimation(characterAnimation);
    }

    /// <summary>
    /// アニメーションの判断を担うメソッド
    /// </summary>
    /// <param name="stateData"></param>
    public void JudgePlayAnimation(StateJudgeBase stateData)
    {
        if (_insertAnimationDictionary.TryGetValue(stateData.GetType(), out MatchTargetAnimationData animationData))
        {
            _characterAnimation.DoAnimation(animationData);
            return;
        }

        if (_loopAnimationDictionary.TryGetValue(stateData.GetType(), out string animationBoolName))
        {
            _characterAnimation.SetAnimationBool(animationBoolName);
        }

    }

    /// <summary>
    /// アニメーションをセットする
    /// </summary>
    /// <param name="characterAnimation">アニメーション情報</param>
    private void SetAnimation(CharacterAnimation characterAnimation)
    {
        // 差し込みアニメーション群
        SetInsertAnimationDictionary<AttackStateJudge>(characterAnimation.AnimationData.AttackAnimation.JabAnimation);
        SetInsertAnimationDictionary<DownStateJudge>(characterAnimation.AnimationData.AttackAnimation.HitAnimation);
        SetInsertAnimationDictionary<GuardHitStateJudge>(characterAnimation.AnimationData.AttackAnimation.GuardHitAnimation);
        SetInsertAnimationDictionary<AvoidanceStateJudge>(characterAnimation.AnimationData.InterruptionAnimation.AvoidanceAnimation);
        SetInsertAnimationDictionary<HitParryStateJudge>(characterAnimation.AnimationData.InterruptionAnimation.HitParryAnimation);
        SetInsertAnimationDictionary<ParryStateJudge>(characterAnimation.AnimationData.InterruptionAnimation.ParryAnimation);

        //Loopアニメーション群
        SetLoopAnimationDictionary<WalkStateJudge>("isWalk");
        SetLoopAnimationDictionary<DashStateJudge>("IsDash");
        SetLoopAnimationDictionary<GuardStateJudge>("IsGuard");

    }

    /// <summary>
    /// アニメーションデータをセットする
    /// </summary>
    /// <typeparam name="T">StateBaseの派生クラス</typeparam>
    /// <param name="animationData">アニメーション</param>
    private void SetInsertAnimationDictionary<T>(MatchTargetAnimationData animationData) where T : StateJudgeBase
    {
        _insertAnimationDictionary.Add(typeof(T), animationData);
    }

    private void SetLoopAnimationDictionary<T>(string animationData) where T : StateJudgeBase
    {
        _loopAnimationDictionary.Add(typeof(T), animationData);
    }
}
