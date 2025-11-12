using UnityEngine;

/// <summary>
/// ダウンステートの判定クラス
/// ダウン状態から通常ステートへのみ遷移可能
/// </summary>
public class DownStateJudge : StateJudgeBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="currentStateChange">ステート変更インターフェース</param>
    public DownStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    /// <summary>
    /// ステート変更をチェックする
    /// 通常ステートへのみ遷移可能
    /// </summary>
    /// <param name="stateType">遷移先のステート</param>
    /// <returns>遷移可能な場合true</returns>
    public override bool CheckChangeState(StateJudgeBase stateType)
    {
        if (stateType is NormalStateJudge)
        {
            _currentStateChange.ChangeState(stateType);
            return true;
        }

        return false;
    }
}
