using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵キャラクターを制御するクラス
/// NavMeshを使用したAI移動、衝突判定、ステートマシンを統合管理する
/// </summary>
public class EnemyCharacter : MonoBehaviour
{
    /// <summary>
    /// 追跡対象のTransform（通常はプレイヤー）
    /// </summary>
    [SerializeField]
    private Transform _target;

    /// <summary>
    /// 自身のTransformキャッシュ
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// 3D衝突判定コンポーネント
    /// </summary>
    private Collision3D _collision3D;

    /// <summary>
    /// 敵のアクション制御コンポーネント
    /// </summary>
    private EnemyAction _enemyAction;

    /// <summary>
    /// 重力適用コンポーネント
    /// </summary>
    private Gravity _playerGravity;

    /// <summary>
    /// 敵の入力制御コンポーネント
    /// </summary>
    private EnemyInput _enemyInput;

    /// <summary>
    /// NavMeshエージェント（AI移動用）
    /// </summary>
    private NavMeshAgent _navMeshAgent;

    /// <summary>
    /// 敵のステートマシン
    /// </summary>
    private EnemyState.StateMachine _stateMachine;

    /// <summary>
    /// NavMeshAgentのプロパティ
    /// </summary>
    public NavMeshAgent NavMeshAgent { get => _navMeshAgent; }

    /// <summary>
    /// 自身のTransformプロパティ
    /// </summary>
    public Transform Transform { get => _transform; }

    /// <summary>
    /// 追跡対象のTransformプロパティ
    /// </summary>
    public Transform TargetTransform { get => _target; }

    /// <summary>
    /// 敵の入力コンポーネントプロパティ
    /// </summary>
    public EnemyInput EnemyInput { get => _enemyInput; }

    /// <summary>
    /// 初期化処理
    /// 必要なコンポーネントを取得し、ステートマシンを構築する
    /// </summary>
    private void Awake()
    {
        _collision3D = this.CheckComponentMissing<Collision3D>();
        _enemyAction = this.CheckComponentMissing<EnemyAction>();
        _playerGravity = this.CheckComponentMissing<Gravity>();
        _enemyInput = this.CheckComponentMissing<EnemyInput>();
        _navMeshAgent = this.CheckComponentMissing<NavMeshAgent>();

        _transform = transform;

        _stateMachine = new EnemyState.StateMachine(this);
    }

    /// <summary>
    /// 開始時の処理
    /// 入力イベントを設定する
    /// </summary>
    private void Start()
    {
        _enemyAction.SetInputEvent(_enemyInput);
    }

    /// <summary>
    /// 毎フレームの更新処理
    /// ステートマシンを実行する
    /// </summary>
    private void Update()
    {
        _stateMachine.ExecuteStateUpdate();
    }

    /// <summary>
    /// 物理演算の更新処理
    /// 衝突判定と重力を適用する
    /// </summary>
    private void FixedUpdate()
    {
        _collision3D.CheckCollision();
        _playerGravity.AdaptationGravity();
    }
}
