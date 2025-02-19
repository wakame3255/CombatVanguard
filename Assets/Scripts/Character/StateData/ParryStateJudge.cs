using System;
using UnityEngine;
public class ParryStateJudge : StateJudgeBase
{

    public ParryStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateJudgeBase stateType)
    {
        _currentStateChange.ChangeState(stateType);
        return true;
    }
}