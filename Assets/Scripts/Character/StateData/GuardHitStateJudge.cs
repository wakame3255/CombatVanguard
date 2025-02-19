using System;
using UnityEngine;

public class GuardHitStateJudge : StateJudgeBase
{
    public GuardHitStateJudge(ICurrentStateChange currentStateChange)
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
        }
        return false;
    }
}