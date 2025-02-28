using System;
using UnityEngine;

public class GuardStateData : StateDataBase
{
    public GuardStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        switch (stateType)
        {
            case GuardStateData:
                return false;
        }

        _currentStateChange.ChangeState(stateType);
        return true;
    }

    public override AnimationClip PlayAnimation(CharacterAnimation characterAnimation)
    {
        characterAnimation.SetAnimationBool(AnimationStringUtility.IsGuardName);

        return null;
    }
}