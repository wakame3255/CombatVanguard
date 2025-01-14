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
        if (_targetTransform != null)
        {
            //目的地の設定
            _agent.SetDestination(_targetTransform.position);

            Vector3 nextPosition = Vector3.zero;

            foreach (Vector3 wayPoint in _agent.path.corners)
            {
                if (Vector3.Distance(wayPoint, _transform.position) > 0.1f)
                {
                    nextPosition = wayPoint;
                    break;
                }
            }
            print(nextPosition);

            //ウェイポイントへの角度計算
            Vector3 nextPointDirection = (nextPosition - _transform.position).normalized;

            //移動入力
            _enemyInput.SetMoveInfomation(new Vector2(nextPointDirection.x, nextPointDirection.z));

            if (Vector3.Distance(_targetTransform.position, _transform.position) < 0.5f)
            {
                _stateMachine.ChangeState(_stateMachine.AttackState);
            }
        }
        else
        {
            _enemyInput.SetMoveInfomation(Vector2.zero);
        }
    }

    public void ExitState()
    {
        _enemyInput.SetMoveInfomation(Vector2.zero);
    }

    private void OnDrawGizmos()
    {
        if (_agent != null && _agent.hasPath)
        {
            // パスの可視化
            Gizmos.color = Color.yellow;
            var corners = _agent.path.corners;
            for (int i = 0; i < corners.Length - 1; i++)
            {
                Gizmos.DrawLine(corners[i], corners[i + 1]);
            }
        }
    }
}
