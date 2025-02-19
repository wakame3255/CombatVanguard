using System;
using UnityEngine;
public class HitParryStateJudge : StateJudgeBase
{

    public HitParryStateJudge(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateJudgeBase stateType)
    {
        switch(stateType)
        {
            case NormalStateJudge:
                _currentStateChange.ChangeState(stateType);
                return true;
            case DownStateJudge:
                _currentStateChange.ChangeState(stateType);
                return true;
        }
        
        return false;
    }
}