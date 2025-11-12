using System;
using UnityEngine;

/// <summary>
/// アニメーションデータを管理するScriptableObject
/// 攻撃アニメーションと割り込みアニメーションの情報を保持する
/// </summary>
[CreateAssetMenu(fileName = "AnimationData", menuName = "Custom/Animation/AnimationData")]
public class AnimationData : ScriptableObject
{
    /// <summary>
    /// 攻撃アニメーション情報
    /// </summary>
    [SerializeField]
    private AttackAnimationInformation attackAnimation;

    /// <summary>
    /// 割り込みアニメーション情報
    /// </summary>
    [SerializeField]
    private InterruptionAnimationInformation interruptionAnimation;

    /// <summary>攻撃アニメーション情報のプロパティ</summary>
    public AttackAnimationInformation AttackAnimation { get => attackAnimation; }

    /// <summary>割り込みアニメーション情報のプロパティ</summary>
    public InterruptionAnimationInformation InterruptionAnimation { get => interruptionAnimation; }
}
