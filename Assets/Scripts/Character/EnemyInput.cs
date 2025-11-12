using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵キャラクターの入力を管理するクラス
/// IInputInformationインターフェースを実装し、ReactivePropertyで入力状態を管理する
/// </summary>
public class EnemyInput : MonoBehaviour, IInputInformation
{
    /// <summary>
    /// 移動入力のReactiveProperty
    /// </summary>
    private ReactiveProperty<Vector2> _reactivePropertyMove = new ReactiveProperty<Vector2>();

    /// <summary>
    /// 攻撃入力のReactiveProperty
    /// </summary>
    private ReactiveProperty<bool> _reactivePropertyAttack = new ReactiveProperty<bool>();

    /// <summary>
    /// 回避入力のReactiveProperty
    /// </summary>
    private ReactiveProperty<bool> _reactivePropertyAvoidance = new ReactiveProperty<bool>();

    /// <summary>
    /// ダッシュ入力のReactiveProperty
    /// </summary>
    private ReactiveProperty<bool> _reactivePropertyDash = new ReactiveProperty<bool>();

    /// <summary>
    /// ガード入力のReactiveProperty
    /// </summary>
    private ReactiveProperty<bool> _reactivePropertyGuard = new ReactiveProperty<bool>();

    /// <summary>
    /// シングルトンインスタンス
    /// </summary>
    public static PlayerInput Instance { get; private set; }

    /// <summary>
    /// 攻撃入力プロパティ
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyAttack { get => _reactivePropertyAttack; }

    /// <summary>
    /// 回避入力プロパティ
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyAvoidance { get => _reactivePropertyAvoidance; }

    /// <summary>
    /// ダッシュ入力プロパティ
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyDash { get => _reactivePropertyDash; }

    /// <summary>
    /// ガード入力プロパティ
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyGuard { get => _reactivePropertyGuard; }

    /// <summary>
    /// 移動入力プロパティ
    /// </summary>
    public ReactiveProperty<Vector2> ReactivePropertyMove { get => _reactivePropertyMove; }

    /// <summary>
    /// 移動情報を設定する
    /// </summary>
    /// <param name="moveDirection">移動方向ベクトル</param>
    public void SetMoveInfomation(Vector2 moveDirection)
    {
        _reactivePropertyMove.Value = moveDirection;
    }

    /// <summary>
    /// 攻撃を実行する
    /// </summary>
    /// <param name="isAttack">攻撃状態</param>
    public void DoAttack(bool isAttack)
    {
        _reactivePropertyAttack.Value = isAttack;    
    }

    /// <summary>
    /// ダッシュを実行する
    /// </summary>
    /// <param name="isDash">ダッシュ状態</param>
    public void DoDash(bool isDash)
    {
        _reactivePropertyDash.Value = isDash;
    }

    /// <summary>
    /// 回避を実行する
    /// </summary>
    /// <param name="isAvoidance">回避状態</param>
    public void DoAvoidance(bool isAvoidance)
    {
        _reactivePropertyAvoidance.Value = isAvoidance;
    }

    /// <summary>
    /// ガードを実行する
    /// </summary>
    /// <param name="isGuard">ガード状態</param>
    public void DoGuard(bool isGuard)
    {
        _reactivePropertyGuard.Value = isGuard;
    }
}
