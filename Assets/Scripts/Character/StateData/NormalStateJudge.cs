using System;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 通常ステートの判定クラス
/// 全てのステートに遷移可能な基本状態
/// </summary>
public class NormalStateJudge : StateJudgeBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="currentStateChange">ステート変更インターフェース</param>
    public NormalStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    /// <summary>
    /// ステート変更をチェックする
    /// 通常状態からは全てのステートに遷移可能
    /// </summary>
    /// <param name="stateType">遷移先のステート</param>
    /// <returns>常にtrue</returns>
    public override bool CheckChangeState(StateJudgeBase stateType)
    {
        _currentStateChange.ChangeState(stateType);
        return true;
    }
}
