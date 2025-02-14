using System;
using UnityEngine;
public class ParryStateData : StateDataBase
{

    public ParryStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        _currentStateChange.ChangeState(stateType);
        return true;
    }

    public override AnimationClip PlayAnimation(CharacterAnimation characterAnimation)
    {
       characterAnimation.DoAnimation(characterAnimation.AnimationData.InterruptionAnimation.ParryAnimation);
        return characterAnimation.AnimationData.InterruptionAnimation.ParryAnimation.AnimationClip;
    }
}