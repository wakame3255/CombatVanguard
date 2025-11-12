using System;

/// <summary>
/// 敵の回避ステートを管理するクラス
/// 回避を実行して即座に移動ステートに戻る
/// </summary>
public class AvoidanceState : IEnemyState
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
    public AvoidanceState(EnemyCharacter enemyCharacter, EnemyState.StateMachine stateMachine)
    {
        _enemyInput = enemyCharacter.EnemyInput;
        _stateMachine = stateMachine;
    }

    /// <summary>
    /// ステートに入るときの処理
    /// 回避を実行して移動ステートに遷移する
    /// </summary>
    public void EnterState()
    {
        _enemyInput.DoAvoidance(true);
        _stateMachine.ChangeState(_stateMachine.MoveState);
    }

    /// <summary>
    /// ステートの更新処理
    /// 回避ステートは即座に遷移するため処理なし
    /// </summary>
    public void UpdateState()
    {

    }

    /// <summary>
    /// ステートから出るときの処理
    /// 回避フラグをリセットする
    /// </summary>
    public void ExitState()
    {
        _enemyInput.DoAvoidance(false);
    }
}