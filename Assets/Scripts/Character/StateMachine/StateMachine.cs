using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyState
{
    public class StateMachine : MonoBehaviour
    {
        private IEnemyState _currentState;
        
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
        public void SwitchToPreviousState()
        {
            ChangeState(_previousState);
        }
    }
}
