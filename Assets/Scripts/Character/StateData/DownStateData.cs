using System;
using UnityEngine;

public class DownStateData : StateDataBase
{
    public DownStateData(ICurrentStateChange currentStateChange)
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
        characterAnimation.DoAnimation(characterAnimation.AnimationData.AttackAnimation.HitAnimation);

        return characterAnimation.AnimationData.AttackAnimation.HitAnimation.AnimationClip;
    }
}