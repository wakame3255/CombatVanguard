using System;
using System.Diagnostics;
using UnityEngine;


public class NormalStateData : StateDataBase
{

    public NormalStateData(ICurrentStateChange currentStateChange)
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
        return null;
    }
}