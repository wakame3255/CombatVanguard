
using UnityEngine;

/// <summary>
/// ステートデータの基底クラス
/// </summary>
public abstract class StateJudgeBase
{
   protected ICurrentStateChange _currentStateChange;

    /// <summary>
    /// 次のステートに遷移できるかチェックする
    /// </summary>
    /// <param name="stateType">次に遷移したいノード</param>
    /// <returns>可能かどうか</returns>
    public abstract bool CheckChangeState(StateJudgeBase stateType);
}
