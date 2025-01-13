using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCharacter : MonoBehaviour
{

    [SerializeField]
    private Transform _target;

    private Transform _transform;
    private Collision3D _collision3D;
    private EnemyAction _enemyAction;
    private Gravity _playerGravity;
    private EnemyInput _enemyInput;
    private NavMeshAgent _navMeshAgent;
    private EnemyState.StateMachine _stateMachine;

    public NavMeshAgent NavMeshAgent { get => _navMeshAgent; }
    public Transform Transform { get => _transform; }
    public Transform TargetTransform { get => _target; }
    public EnemyInput EnemyInput { get => _enemyInput; }

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
    private void Start()
    {
        _enemyAction.SetInputEvent(_enemyInput);
    }

    private void Update()
    {
        _stateMachine.ExecuteStateUpdate();
    }

    private void FixedUpdate()
    {
        _collision3D.CheckCollision();
        _playerGravity.AdaptationGravity();
    }
}
