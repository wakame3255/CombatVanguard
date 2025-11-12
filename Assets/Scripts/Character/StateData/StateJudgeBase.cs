using UnityEngine;

/// <summary>
/// ステートデータの基底クラス
/// 全てのステート判定クラスが継承する
/// </summary>
public abstract class StateJudgeBase
{
   /// <summary>
   /// ステート変更インターフェース
   /// </summary>
   protected ICurrentStateChange _currentStateChange;

    /// <summary>
    /// 他のステートに遷移できるかチェックする
    /// </summary>
    /// <param name="stateType">遷移先のステート</param>
    /// <returns>遷移可能かどうか</returns>
    public abstract bool CheckChangeState(StateJudgeBase stateType);
}
