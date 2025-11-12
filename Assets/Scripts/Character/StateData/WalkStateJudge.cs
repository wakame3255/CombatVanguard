using System;
using UnityEngine;

/// <summary>
/// 歩行ステートの判定クラス
/// 歩行状態から他のステートへの遷移を制御する
/// </summary>
public class WalkStateJudge : StateJudgeBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="currentStateChange">ステート変更インターフェース</param>
    public WalkStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    /// <summary>
    /// ステート変更をチェックする
    /// 同じ歩行ステートへの遷移は不可
    /// </summary>
    /// <param name="stateType">遷移先のステート</param>
    /// <returns>遷移可能な場合true</returns>
    public override bool CheckChangeState(StateJudgeBase stateType)
    {
        switch (stateType)
        {
            case WalkStateJudge:
                return false;
        }

        _currentStateChange.ChangeState(stateType);
        return true;
    }
}
