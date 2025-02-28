using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class AttackStateData : StateDataBase
{
    private bool _isJab;
    public AttackStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        switch (stateType)
        {
            case NormalStateData:
                _currentStateChange.ChangeState(stateType);
                return true;
            case DownStateData:
                _currentStateChange.ChangeState(stateType);
                return true;

            case HitParryStateData:
                _currentStateChange.ChangeState(stateType);
                return true;
        }

        return false;
    }

    public override AnimationClip PlayAnimation(CharacterAnimation characterAnimation)
    {
        if (_isJab)
        {
            characterAnimation.DoAnimation(characterAnimation.AnimationData.AttackAnimation.JabAnimation);
            _isJab = false;
            return characterAnimation.AnimationData.AttackAnimation.JabAnimation.AnimationClip;
        }
        else
        {
            characterAnimation.DoAnimation(characterAnimation.AnimationData.AttackAnimation.MirrorJabAnimation);
            _isJab = true;
            return characterAnimation.AnimationData.AttackAnimation.MirrorJabAnimation.AnimationClip;
        }
    }
}