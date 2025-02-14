using System;
using UnityEngine;

public class AvoidanceStateData : StateDataBase
{

    public AvoidanceStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        if (stateType is NormalStateData)
        {
            _currentStateChange.ChangeState(stateType);
            return true;
        }

        return false;
    }

    public override AnimationClip PlayAnimation(CharacterAnimation characterAnimation)
    {
       characterAnimation.DoAnimation(characterAnimation.AnimationData.InterruptionAnimation.AvoidanceAnimation);

        return characterAnimation.AnimationData.InterruptionAnimation.AvoidanceAnimation.AnimationClip;
    }
}