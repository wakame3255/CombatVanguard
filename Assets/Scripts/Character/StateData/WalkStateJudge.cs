using System;
using UnityEngine;

public class WalkStateJudge : StateJudgeBase
{

    public WalkStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateJudgeBase stateType)
    {
        switch (stateType)
        {
            case WalkStateJudge:
                return false;
        }

        _currentStateChange.ChangeState(stateType);
        return true;
    }
}