using System;

public class GuardStateData : StateDataBase
{
    public GuardStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        return true;
    }

    public override void PlayAnimation(CharacterAnimation characterAnimation)
    {

    }
}