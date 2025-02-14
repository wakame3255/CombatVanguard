using System;
using UnityEngine;

public class GuardHitStateData : StateDataBase
{
    public GuardHitStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        switch (stateType)
        {
            case GuardStateData:
                _currentStateChange.ChangeState(stateType);
                return true;
        }
        return false;
    }

    public override AnimationClip PlayAnimation(CharacterAnimation characterAnimation)
    {
        characterAnimation.DoAnimation(characterAnimation.AnimationData.AttackAnimation.GuardHitAnimation);

        return characterAnimation.AnimationData.AttackAnimation.GuardHitAnimation.AnimationClip;
    }
}