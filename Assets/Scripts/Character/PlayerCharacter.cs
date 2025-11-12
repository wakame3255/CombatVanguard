using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーキャラクターを制御するクラス
/// 衝突判定、重力、アクション入力を統合管理する
/// </summary>
[RequireComponent(typeof(Collision3D))]
[RequireComponent(typeof(Gravity))]
public class PlayerCharacter : MonoBehaviour
{
    /// <summary>
    /// 3D衝突判定コンポーネント
    /// </summary>
    private Collision3D _collision3D;

    /// <summary>
    /// プレイヤーアクションコンポーネント
    /// </summary>
    private PlayerAction _playerAction;

    /// <summary>
    /// 重力適用コンポーネント
    /// </summary>
    private Gravity _playerGravity;

    /// <summary>
    /// 初期化処理
    /// 必要なコンポーネントを取得する
    /// </summary>
    private void Awake()
    {
        _collision3D = this.CheckComponentMissing<Collision3D>();
        _playerAction = this.CheckComponentMissing<PlayerAction>();
        _playerGravity = this.CheckComponentMissing<Gravity>();
    }

    /// <summary>
    /// 開始時の処理
    /// 入力イベントを設定する
    /// </summary>
    private void Start()
    {
        _playerAction.SetInputEvent(PlayerInput.Instance);
    }

    /// <summary>
    /// 物理演算の更新処理
    /// 衝突判定と重力を適用する
    /// </summary>
    private void FixedUpdate()
    {
        _collision3D.CheckCollision();
        _playerGravity.AdaptationGravity();
    }
}
