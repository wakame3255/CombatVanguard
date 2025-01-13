using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyState
{
    public class StateMachine
    {
        private IEnemyState _currentState;

        private MoveState _moveState;


        public StateMachine(EnemyInput enemyInput)
        {
            _moveState = new MoveState(enemyInput, this);

            ChangeState(_moveState);
             Debug.Log("StateMachine");
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
