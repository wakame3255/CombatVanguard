using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の攻撃ステートを管理するクラス
/// 攻撃を実行して即座に移動ステートに戻る
/// </summary>
public class AttackState :  IEnemyState
{
    /// <summary>
    /// 敵の入力コンポーネント
    /// </summary>
    private EnemyInput _enemyInput;

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
    public AttackState(EnemyCharacter enemyCharacter, EnemyState.StateMachine stateMachine)
    {
        _enemyInput = enemyCharacter.EnemyInput;
        _stateMachine = stateMachine;
    }

    /// <summary>
    /// ステートに入るときの処理
    /// 攻撃を実行して移動ステートに遷移する
    /// </summary>
    public void EnterState()
    {
        _enemyInput.DoAttack(true);

        _stateMachine.ChangeState(_stateMachine.MoveState);
    }

    /// <summary>
    /// ステートの更新処理
    /// 攻撃ステートは即座に遷移するため処理なし
    /// </summary>
    public void UpdateState()
    {
       
    }

    /// <summary>
    /// ステートから出るときの処理
    /// 攻撃フラグをリセットする
    /// </summary>
    public void ExitState()
    {
        _enemyInput.DoAttack(false);
    }
}
