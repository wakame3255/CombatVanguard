using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCharacter : MonoBehaviour
{

    private Collision3D _collision3D;
    private EnemyAction _enemyAction;
    private Gravity _playerGravity;
    private EnemyInput _enemyInput;
    private NavMeshAgent _navMeshAgent;
    private EnemyState.StateMachine _stateMachine;

    private void Awake()
    {
        _collision3D = this.CheckComponentMissing<Collision3D>();
        _enemyAction = this.CheckComponentMissing<EnemyAction>();
        _playerGravity = this.CheckComponentMissing<Gravity>();
        _enemyInput = this.CheckComponentMissing<EnemyInput>();
        _navMeshAgent = this.CheckComponentMissing<NavMeshAgent>();

        _stateMachine = new EnemyState.StateMachine(_enemyInput, _navMeshAgent);
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
