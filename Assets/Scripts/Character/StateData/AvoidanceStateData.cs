using System;

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

    public override void PlayAnimation(CharacterAnimation characterAnimation)
    {
       characterAnimation.DoAvoidanceAnimation();
    }
}