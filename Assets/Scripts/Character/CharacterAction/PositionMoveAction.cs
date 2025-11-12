using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの位置移動アクションを管理するクラス
/// 歩行、ダッシュの移動速度を制御し、ターン動作も処理する
/// </summary>
public class PositionMoveAction : MonoBehaviour, ISetTransform, ISetAnimation
{
    /// <summary>
    /// 歩行速度
    /// </summary>
    [SerializeField]
    private float _walkSpeed;

    /// <summary>
    /// ダッシュ速度
    /// </summary>
    [SerializeField]
    private float _dashSpeed;

    /// <summary>
    /// キャラクターのTransform
    /// </summary>
    private Transform _characterTransform;

    /// <summary>
    /// キャラクターアニメーションコンポーネント
    /// </summary>
    private CharacterAnimation _characterAnimation;

    /// <summary>
    /// 前フレームの移動方向をキャッシュ
    /// </summary>
    private Vector3 _cacheMoveDirecton;

    /// <summary>
    /// ターン判定の角度閾値
    /// </summary>
    private const float TURN_ANGLE = 100f;

    /// <summary>
    /// 移動処理を実行するメソッド
    /// 現在のステートに応じて適切な移動速度とアニメーションを適用する
    /// </summary>
    /// <param name="moveDirection">移動方向ベクトル</param>
    /// <param name="characterState">キャラクターのステート</param>
    public void DoMove(Vector3 moveDirection, IApplicationStateChange characterState)
    {
        switch (characterState.CurrentStateData)
        {
            case WalkStateJudge or GuardStateJudge:
                DoMovePosition(moveDirection, _walkSpeed);
                _characterAnimation.DoWalkAnimation(moveDirection);
                break;

            case DashStateJudge:
                CheckDashTurn(moveDirection);
                DoMovePosition(moveDirection, _dashSpeed);
                _characterAnimation.DoWalkAnimation(moveDirection);
                break;
        }
        _cacheMoveDirecton = moveDirection;
    }
   
    /// <summary>
    /// キャラクターのTransformを設定する
    /// </summary>
    /// <param name="characterTransform">キャラクターのTransform</param>
    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    /// <summary>
    /// アニメーションコンポーネントを設定する
    /// </summary>
    /// <param name="characterAnimation">キャラクターアニメーション</param>
    public void SetAnimationComponent(CharacterAnimation characterAnimation)
    {
        _characterAnimation = characterAnimation;
    }

    /// <summary>
    /// 実際の位置移動を行う
    /// 指定された方向と速度で移動する
    /// </summary>
    /// <param name="moveDirection">移動方向</param>
    /// <param name="moveSpeed">移動速度</param>
    private void DoMovePosition(Vector3 moveDirection, float moveSpeed)
    {
        _characterTransform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// ダッシュ中のターン判定を行うメソッド
    /// 急激な方向転換時にターンアニメーションを再生する
    /// </summary>
    /// <param name="moveDirection">移動方向</param>
    private void CheckDashTurn(Vector3 moveDirection)
    {
        if (Vector3.Angle(moveDirection, _cacheMoveDirecton) > TURN_ANGLE)
        {
            _characterAnimation.DoAnimation(_characterAnimation.AnimationData.InterruptionAnimation.TurnAnimation);
        }
    }
}
