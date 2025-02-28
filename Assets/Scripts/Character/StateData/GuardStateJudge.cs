using System;
using UnityEngine;

public class GuardStateJudge : StateJudgeBase
{
    public GuardStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

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