using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UnityEngine;
using R3;

public class PlayerAction : MonoBehaviour
{
    [SerializeField, Required] [Header("アクションを置く親")]
    private GameObject _actionPosition;

    private MoveAction _moveAction;
    private AttackAction _attackAction;

    private void Awake()
    {
        _moveAction = this.CheckComponentMissing<MoveAction>(_actionPosition);
        _moveAction.SetTransform(transform);
        _attackAction = this.CheckComponentMissing<AttackAction>(_actionPosition);
    }

    public void SetInputEvent(IInputInformation inputInformation)
    {
        MyExtensionClass.CheckArgumentNull(inputInformation, nameof(inputInformation));

        inputInformation.ReactivePropertyMove.Subscribe(inputXY => _moveAction.DoMove(inputXY));
        inputInformation.ReactivePropertyAttack.Where(isAttack => isAttack).Subscribe(isAttack => _attackAction.DoAction());
        inputInformation.ReactivePropertyJump.Where(isJump => isJump).Subscribe(isJump => print("jump"));
    }
}
