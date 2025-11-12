using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

/// <summary>
/// 入力情報を提供するインターフェース
/// プレイヤーと敵の入力を統一的に扱うための共通インターフェース
/// </summary>
public interface IInputInformation
{
    /// <summary>
    /// 攻撃入力のReactiveProperty
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyAttack { get; }

    /// <summary>
    /// 回避入力のReactiveProperty
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyAvoidance { get; }

    /// <summary>
    /// ダッシュ入力のReactiveProperty
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyDash { get; }

    /// <summary>
    /// ガード入力のReactiveProperty
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyGuard { get; }

    /// <summary>
    /// 移動入力のReactiveProperty
    /// </summary>
    public ReactiveProperty<Vector2> ReactivePropertyMove { get; }
}
