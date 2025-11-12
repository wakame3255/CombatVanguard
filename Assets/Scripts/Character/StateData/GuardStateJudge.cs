using System;
using UnityEngine;

/// <summary>
/// ガードステートの判定クラス
/// ガード状態から他のステートへの遷移を制御する
/// </summary>
public class GuardStateJudge : StateJudgeBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="currentStateChange">ステート変更インターフェース</param>
    public GuardStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    /// <summary>
    /// ステート変更をチェックする
    /// 同じガードステートへの遷移は不可
    /// </summary>
    /// <param name="stateType">遷移先のステート</param>
    /// <returns>遷移可能な場合true</returns>
    public override bool CheckChangeState(StateJudgeBase stateType)
    {
        switch (stateType)
        {
            case GuardStateJudge:
                return false;
        }

        _currentStateChange.ChangeState(stateType);
        return true;
    }
}
