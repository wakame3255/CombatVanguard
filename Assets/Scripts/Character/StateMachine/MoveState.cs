using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵の移動ステートを管理するクラス
/// NavMeshを使用してターゲットに向かって移動し、近づいたら攻撃または回避ステートに遷移する
/// </summary>
public class MoveState : IEnemyState
{
    /// <summary>
    /// 敵自身のTransform
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// 追跡対象のTransform
    /// </summary>
    private Transform _targetTransform;

    /// <summary>
    /// 敵の入力コンポーネント
    /// </summary>
    private EnemyInput _enemyInput;

    /// <summary>
    /// NavMeshエージェント
    /// </summary>
    private NavMeshAgent _agent;

    /// <summary>
    /// ステートマシン
    /// </summary>
    private EnemyState.StateMachine _stateMachine;

    /// <summary>
    /// コンストラクタ
    /// 必要なコンポーネントを初期化する
    /// </summary>
    /// <param name="enemyCharacter">敵キャラクター</param>
    /// <param name="stateMachine">ステートマシン</param>
    public MoveState(EnemyCharacter enemyCharacter, EnemyState.StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _agent = enemyCharacter.NavMeshAgent;
        _enemyInput = enemyCharacter.EnemyInput;
        _transform = enemyCharacter.Transform;
        _targetTransform = enemyCharacter.TargetTransform;
    }

    /// <summary>
    /// ステートに入るときの処理
    /// </summary>
    public void EnterState()
    {

    }

    /// <summary>
    /// ステートの更新処理
    /// ターゲットに向かって移動し、一定距離以内に近づいたら次のステートに遷移する
    /// </summary>
    public void UpdateState()
    {
        if (_targetTransform != null)
        {
            // 移動目的地の設定
            _agent.SetDestination(_targetTransform.position);

            Vector3 nextPosition = Vector3.zero;

            // 次のウェイポイントを探す
            foreach (Vector3 wayPoint in _agent.path.corners)
            {
                if (Vector3.Distance(wayPoint, _transform.position) > 0.1f)
                {
                    nextPosition = wayPoint;
                    break;
                }
            }

            // ウェイポイントへの角度を計算
            Vector3 nextPointDirection = (nextPosition - _transform.position).normalized;

            // 移動入力を設定
            _enemyInput.SetMoveInfomation(new Vector2(nextPointDirection.x, nextPointDirection.z));

            // ターゲットに十分近づいたら攻撃または回避ステートに遷移
            if (Vector3.Distance(_targetTransform.position, _transform.position) < 1f)
            {
                int random = Random.Range(0, 2);
                switch (random)
                {
                    case 0:
                        _stateMachine.ChangeState(_stateMachine.AvoidanceState);
                        break;
                    case 1:
                        _stateMachine.ChangeState(_stateMachine.AttackState);
                        break;
                }

            }
        }
        else
        {
            _enemyInput.SetMoveInfomation(Vector2.zero);
        }
    }

    /// <summary>
    /// ステートから出るときの処理
    /// 移動入力をゼロにする
    /// </summary>
    public void ExitState()
    {
        _enemyInput.SetMoveInfomation(Vector2.zero);
    }

    /// <summary>
    /// Gizmosの描画処理
    /// NavMeshのパスを可視化する
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_agent != null && _agent.hasPath)
        {
            // パスの描画
            Gizmos.color = Color.yellow;
            var corners = _agent.path.corners;
            for (int i = 0; i < corners.Length - 1; i++)
            {
                Gizmos.DrawLine(corners[i], corners[i + 1]);
            }
        }
    }
}
