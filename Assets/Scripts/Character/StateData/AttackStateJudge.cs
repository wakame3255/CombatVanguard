using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class AttackStateJudge : StateJudgeBase
{
    private bool _isJab;
    public AttackStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

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