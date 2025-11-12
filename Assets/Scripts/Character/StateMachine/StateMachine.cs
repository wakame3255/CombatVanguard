using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyState
{
    /// <summary>
    /// 敵のステートマシンを管理するクラス
    /// 移動、攻撃、回避の各ステート間の遷移を制御する
    /// </summary>
    public class StateMachine
    {
        /// <summary>
        /// 現在のステート
        /// </summary>
        private IEnemyState _currentState;

        /// <summary>
        /// 移動ステート
        /// </summary>
        private MoveState _moveState;

        /// <summary>
        /// 攻撃ステート
        /// </summary>
        private AttackState _attackState;

        /// <summary>
        /// 回避ステート
        /// </summary>
        private AvoidanceState _avoidanceState;

        /// <summary>
        /// 移動ステートのプロパティ
        /// </summary>
        public MoveState MoveState { get => _moveState; }

        /// <summary>
        /// 攻撃ステートのプロパティ
        /// </summary>
        public AttackState AttackState { get => _attackState; }

        /// <summary>
        /// 回避ステートのプロパティ
        /// </summary>
        public AvoidanceState AvoidanceState { get => _avoidanceState; }

        /// <summary>
        /// コンストラクタ
        /// 各ステートを初期化し、初期ステートを移動ステートに設定する
        /// </summary>
        /// <param name="enemyCharacter">敵キャラクター</param>
        public StateMachine(EnemyCharacter enemyCharacter)
        {
            _moveState = new MoveState(enemyCharacter, this);
            _attackState = new AttackState(enemyCharacter, this);
            _avoidanceState = new AvoidanceState(enemyCharacter, this);

            ChangeState(_moveState);
        }

        /// <summary>
        /// ステートを変更する
        /// 現在のステートを終了し、新しいステートに入る
        /// </summary>
        /// <param name="newState">新しいステート</param>
        public void ChangeState(IEnemyState newState)
        {
            if (newState != null)
            {
                newState.ExitState();
            }
            _currentState = newState;
            _currentState.EnterState();
        }

        /// <summary>
        /// 現在のステートの更新処理を実行する
        /// </summary>
        public void ExecuteStateUpdate()
        {
            if (_currentState != null)
            {
                _currentState.UpdateState();
            }
        }
    }
}
