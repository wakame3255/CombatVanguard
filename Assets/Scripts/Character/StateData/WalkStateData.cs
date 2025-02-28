using System;
using UnityEngine;

public class WalkStateData : StateDataBase
{

    public WalkStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        switch (stateType)
        {
            case WalkStateData:
                return false;
        }

        _currentStateChange.ChangeState(stateType);
        return true;
    }

    public override AnimationClip PlayAnimation(CharacterAnimation characterAnimation)
    {
       characterAnimation.SetAnimationBool("");

        return null;
    }
}