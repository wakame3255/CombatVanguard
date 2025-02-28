using System;
using UnityEngine;

public class DashStateJudge : StateJudgeBase
{

    public DashStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

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