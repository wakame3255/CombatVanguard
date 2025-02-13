using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class AttackStateData : StateDataBase
{
    bool _isJab;
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
        }

        return false;
    }

    public override void PlayAnimation(CharacterAnimation characterAnimation)
    {
        if (_isJab)
        {
            characterAnimation.DoAnimation(characterAnimation.AnimationData.AttackAnimation.JabAnimation);
            _isJab = false;
        }
        else
        {
            characterAnimation.DoAnimation(characterAnimation.AnimationData.AttackAnimation.MirrorJabAnimation);
            _isJab = true;
        }
    }
}