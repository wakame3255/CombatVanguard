using System;
using System.Diagnostics;
using UnityEngine;


public class NormalStateJudge : StateJudgeBase
{

    public NormalStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateJudgeBase stateType)
    {
        _currentStateChange.ChangeState(stateType);
        return true;
    }
}