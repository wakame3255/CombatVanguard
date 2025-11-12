using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 攻撃ステートの判定クラス
/// 攻撃状態から特定のステートへの遷移のみを許可する
/// </summary>
public class AttackStateJudge : StateJudgeBase
{
    /// <summary>
    /// ジャブ攻撃かどうかのフラグ
    /// </summary>
    private bool _isJab;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="currentStateChange">ステート変更インターフェース</param>
    public AttackStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    /// <summary>
    /// ステート変更をチェックする
    /// 通常、ダウン、パリィ被弾ステートへのみ遷移可能
    /// </summary>
    /// <param name="stateType">遷移先のステート</param>
    /// <returns>遷移可能な場合true</returns>
    public override bool CheckChangeState(StateJudgeBase stateType)
    {
        switch (stateType)
        {
            case NormalStateJudge:
                _currentStateChange.ChangeState(stateType);
                return true;
            case DownStateJudge:
                _currentStateChange.ChangeState(stateType);
                return true;

            case HitParryStateJudge:
                _currentStateChange.ChangeState(stateType);
                return true;
        }

        return false;
    }
}
