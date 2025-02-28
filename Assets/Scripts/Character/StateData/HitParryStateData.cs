using System;
using UnityEngine;
public class HitParryStateData : StateDataBase
{

    public HitParryStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        switch(stateType)
        {
            case NormalStateData:
                _currentStateChange.ChangeState(stateType);
                return true;
            case DownStateData:
                _currentStateChange.ChangeState(stateType);
                return true;
        }
        
        return false;
    }

    public override AnimationClip PlayAnimation(CharacterAnimation characterAnimation)
    {
        characterAnimation.DoAnimation(characterAnimation.AnimationData.InterruptionAnimation.HitParryAnimation);
        return characterAnimation.AnimationData.InterruptionAnimation.HitParryAnimation.AnimationClip;
    }
}