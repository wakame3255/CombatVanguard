using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IEnemyState
{
    private EnemyInput _enemyInput;
    private EnemyState.StateMachine _stateMachine;

    public AttackState(EnemyCharacter enemyCharacter, EnemyState.StateMachine stateMachine)
    {
        _enemyInput = enemyCharacter.EnemyInput;
        _stateMachine = stateMachine;
    }

    public void EnterState()
    {
        _enemyInput.DoAttack(true);
        _stateMachine.ChangeState(_stateMachine.MoveState);
    }

    public void UpdateState()
    {
       
    }

    public void ExitState()
    {
        _enemyInput.DoAttack(false);
    }
}
