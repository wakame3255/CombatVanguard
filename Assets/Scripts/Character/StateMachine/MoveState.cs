using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : MonoBehaviour, IEnemyState
{
    private EnemyInput _enemyInput;
    private EnemyState.StateMachine _stateMachine;

    public MoveState(EnemyInput enemyInput, EnemyState.StateMachine stateMachine)
    {
        _enemyInput = enemyInput;
        _stateMachine = stateMachine;
    }

    public void EnterState()
    {

    }

    public void UpdateState()
    {
        _enemyInput.SetMoveInfomation(new Vector2(1f,0f));
    }

    public void ExitState()
    {

    }
}
