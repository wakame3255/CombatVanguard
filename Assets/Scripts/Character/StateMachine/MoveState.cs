using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : MonoBehaviour, IEnemyState
{
    private EnemyInput _enemyInput;
    private NavMeshAgent _agent;
    private EnemyState.StateMachine _stateMachine;

    public MoveState(EnemyInput enemyInput, EnemyState.StateMachine stateMachine, NavMeshAgent navMeshAgent)
    {
        _enemyInput = enemyInput;
        _stateMachine = stateMachine;
        _agent = navMeshAgent;
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
