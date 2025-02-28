using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMoveAction : MonoBehaviour, ISetTransform, ISetAnimation
{
    [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _dashSpeed;

    private Transform _characterTransform;
    private CharacterAnimation _characterAnimation;

    private Vector3 _cacheMoveDirecton;

    private const float TURN_ANGLE = 100f;

    /// <summary>
    /// 動くメソッド
    /// </summary>
    /// <param name="moveDirection">移動する方向</param>
    public void DoMove(Vector3 moveDirection, IApplicationStateChange characterState)
    {
        switch (characterState.CurrentStateData)
        {
            case WalkStateData or GuardStateData:
                DoMovePosition(moveDirection, _walkSpeed);
                _characterAnimation.DoWalkAnimation(moveDirection);
                break;

            case DashStateData:
                CheckDashTurn(moveDirection);
                DoMovePosition(moveDirection, _dashSpeed);
                _characterAnimation.DoWalkAnimation(moveDirection);
                break;
        }
        _cacheMoveDirecton = moveDirection;
    }
   
    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    public void SetAnimationComponent(CharacterAnimation characterAnimation)
    {
        _characterAnimation = characterAnimation;
    }

    private void DoMovePosition(Vector3 moveDirection, float moveSpeed)
    {
        _characterTransform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// ターンを行う判定メソッド
    /// </summary>
    /// <param name="moveDirection"></param>
    private void CheckDashTurn(Vector3 moveDirection)
    {
        if (Vector3.Angle(moveDirection, _cacheMoveDirecton) > TURN_ANGLE)
        {
            _characterAnimation.DoAnimation(_characterAnimation.AnimationData.InterruptionAnimation.TurnAnimation);
        }
    }
}
