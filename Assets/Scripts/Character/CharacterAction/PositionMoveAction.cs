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

    private bool _isDash;

    /// <summary>
    /// “®‚­ƒƒ\ƒbƒh
    /// </summary>
    /// <param name="moveDirection">ˆÚ“®‚·‚é•ûŒü</param>
    public void DoMove(Vector3 moveDirection)
    {
        if (_isDash)
        {
            CheckDashTurn(moveDirection);
            DoMovePosition(moveDirection, _dashSpeed);
            _characterAnimation.DoDashAnimation(moveDirection);        
        }    
        else
        {
            DoMovePosition(moveDirection, _walkSpeed); 
            _characterAnimation.DoWalkAnimation(moveDirection);
        }
    }

    public void SetDashTrigger(bool isDash)
    {
        _isDash = isDash;
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

    private void CheckDashTurn(Vector3 moveDirection)
    {
        if (Vector3.Angle(moveDirection, _cacheMoveDirecton) > 120f)
        {
            _characterAnimation.DoTurnAnimation();
        }
        _cacheMoveDirecton = moveDirection;
    }
}
