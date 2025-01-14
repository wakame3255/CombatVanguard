using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyState
{
    public class StateMachine
    {
        private IEnemyState _currentState;

        private MoveState _moveState;
        private AttackState _attackState;

        public MoveState MoveState { get => _moveState; }
        public AttackState AttackState { get => _attackState; }

        public StateMachine(EnemyCharacter enemyCharacter)
        {
            _moveState = new MoveState(enemyCharacter, this);
            _attackState = new AttackState(enemyCharacter, this);

            ChangeState(_moveState);
        }

        public void ChangeState(IEnemyState newState)
        {
            if (newState != null)
            {
                newState.ExitState();
            }
            _currentState = newState;
            _currentState.EnterState();
        }
        public void ExecuteStateUpdate()
        {
            if (_currentState != null)
            {
                _currentState.UpdateState();
            }
        }
    }
}
