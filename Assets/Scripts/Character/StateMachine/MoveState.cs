using UnityEngine;
using UnityEngine.AI;

public class MoveState : MonoBehaviour, IEnemyState
{
    private Transform _transform;
    private Transform _targetTransform;
    private EnemyInput _enemyInput;
    private NavMeshAgent _agent;
    private EnemyState.StateMachine _stateMachine;

    public MoveState(EnemyCharacter enemyCharacter, EnemyState.StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _agent = enemyCharacter.NavMeshAgent;
        _enemyInput = enemyCharacter.EnemyInput;
        _transform = enemyCharacter.Transform;
        _targetTransform = enemyCharacter.TargetTransform;

    }

    public void EnterState()
    {

    }

    public void UpdateState()
    {
        Vector3 targetDirection;

        if (_targetTransform != null)
        {
            targetDirection = (_targetTransform.position - _transform.position).normalized;
            _enemyInput.SetMoveInfomation(new Vector2(targetDirection.x, targetDirection.z));
        }
        else
        {
            _enemyInput.SetMoveInfomation(Vector2.zero);
        }
    }

    public void ExitState()
    {

    }
}
