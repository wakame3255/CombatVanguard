using System;
using UnityEngine;

public class AvoidanceStateJudge : StateJudgeBase
{

    public AvoidanceStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

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