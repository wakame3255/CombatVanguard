using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートに応じてアニメーションを判定するクラス
/// ステートの種類に基づいて適切なアニメーションを再生する
/// </summary>
public class CharacterAnimationJudge
{
    /// <summary>
    /// 挿入型アニメーション（一度だけ再生）の辞書
    /// ステート型をキーとしてアニメーションデータを保持
    /// </summary>
    private Dictionary<Type, MatchTargetAnimationData> _insertAnimationDictionary;

    /// <summary>
    /// ループアニメーションの辞書
    /// ステート型をキーとしてAnimatorのBoolパラメータ名を保持
    /// </summary>
    private Dictionary<Type, String> _loopAnimationDictionary;

    /// <summary>
    /// キャラクターアニメーションコンポーネント
    /// </summary>
    private CharacterAnimation _characterAnimation;

    /// <summary>
    /// コンストラクタ
    /// アニメーション辞書を初期化し、アニメーションデータを設定する
    /// </summary>
    /// <param name="characterAnimation">キャラクターアニメーション</param>
    public CharacterAnimationJudge(CharacterAnimation characterAnimation)
    {
        _insertAnimationDictionary = new Dictionary<Type, MatchTargetAnimationData>();
        _loopAnimationDictionary = new Dictionary<Type, string>();

        _characterAnimation = characterAnimation;

        SetAnimation(characterAnimation);
    }

    /// <summary>
    /// アニメーションの判定を行う総合メソッド
    /// ステートに応じて挿入型またはループ型アニメーションを再生する
    /// </summary>
    /// <param name="stateData">現在のステートデータ</param>
    public void JudgePlayAnimation(StateJudgeBase stateData)
    {
        // 挿入型アニメーションをチェック
        if (_insertAnimationDictionary.TryGetValue(stateData.GetType(), out MatchTargetAnimationData animationData))
        {
            _characterAnimation.DoAnimation(animationData);
            return;
        }

        // ループ型アニメーションをチェック
        if (_loopAnimationDictionary.TryGetValue(stateData.GetType(), out string animationBoolName))
        {
            _characterAnimation.SetAnimationBool(animationBoolName);
        }

    }

    /// <summary>
    /// アニメーションをセットする
    /// 各ステートに対応するアニメーションを辞書に登録する
    /// </summary>
    /// <param name="characterAnimation">アニメーションデータ</param>
    private void SetAnimation(CharacterAnimation characterAnimation)
    {
        // 挿入型アニメーション登録
        SetInsertAnimationDictionary<AttackStateJudge>(characterAnimation.AnimationData.AttackAnimation.JabAnimation);
        SetInsertAnimationDictionary<DownStateJudge>(characterAnimation.AnimationData.AttackAnimation.HitAnimation);
        SetInsertAnimationDictionary<GuardHitStateJudge>(characterAnimation.AnimationData.AttackAnimation.GuardHitAnimation);
        SetInsertAnimationDictionary<AvoidanceStateJudge>(characterAnimation.AnimationData.InterruptionAnimation.AvoidanceAnimation);
        SetInsertAnimationDictionary<HitParryStateJudge>(characterAnimation.AnimationData.InterruptionAnimation.HitParryAnimation);
        SetInsertAnimationDictionary<ParryStateJudge>(characterAnimation.AnimationData.InterruptionAnimation.ParryAnimation);

        // ループ型アニメーション登録
        SetLoopAnimationDictionary<WalkStateJudge>("isWalk");
        SetLoopAnimationDictionary<DashStateJudge>("IsDash");
        SetLoopAnimationDictionary<GuardStateJudge>("IsGuard");

    }

    /// <summary>
    /// 挿入型アニメーションデータを辞書にセットする
    /// </summary>
    /// <typeparam name="T">StateBaseの派生クラス</typeparam>
    /// <param name="animationData">アニメーションデータ</param>
    private void SetInsertAnimationDictionary<T>(MatchTargetAnimationData animationData) where T : StateJudgeBase
    {
        _insertAnimationDictionary.Add(typeof(T), animationData);
    }

    /// <summary>
    /// ループ型アニメーションのパラメータ名を辞書にセットする
    /// </summary>
    /// <typeparam name="T">StateBaseの派生クラス</typeparam>
    /// <param name="animationData">Animatorのパラメータ名</param>
    private void SetLoopAnimationDictionary<T>(string animationData) where T : StateJudgeBase
    {
        _loopAnimationDictionary.Add(typeof(T), animationData);
    }
}
