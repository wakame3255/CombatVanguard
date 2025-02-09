using System;

public class AvoidanceState : IEnemyState
{
    private EnemyInput _enemyInput;
    private EnemyState.StateMachine _stateMachine;

    public AvoidanceState(EnemyCharacter enemyCharacter, EnemyState.StateMachine stateMachine)
    {
        _enemyInput = enemyCharacter.EnemyInput;
        _stateMachine = stateMachine;
    }

    public void EnterState()
    {
        _enemyInput.DoAvoidance(true);
        _stateMachine.ChangeState(_stateMachine.MoveState);
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {
        _enemyInput.DoAvoidance(false);
    }
}