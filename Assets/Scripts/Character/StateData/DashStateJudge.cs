using System;
using UnityEngine;

/// <summary>
/// ダッシュステートの判定クラス
/// ダッシュ状態から他のステートへの遷移を制御する
/// </summary>
public class DashStateJudge : StateJudgeBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="currentStateChange">ステート変更インターフェース</param>
    public DashStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    /// <summary>
    /// ステート変更をチェックする
    /// 同じダッシュステートへの遷移は不可
    /// </summary>
    /// <param name="stateType">遷移先のステート</param>
    /// <returns>遷移可能な場合true</returns>
    public override bool CheckChangeState(StateJudgeBase stateType)
    {
        switch (stateType)
        {
            case DashStateJudge:
                return false;
        }

        _currentStateChange.ChangeState(stateType);
        return true;
    }
}
