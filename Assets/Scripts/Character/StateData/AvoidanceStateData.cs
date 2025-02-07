using System;

public class AvoidanceStateData : StateDataBase
{

    public AvoidanceStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        throw new NotImplementedException();
    }

    public override void PlayAnimation(CharacterAnimation characterAnimation)
    {
        throw new NotImplementedException();
    }
}