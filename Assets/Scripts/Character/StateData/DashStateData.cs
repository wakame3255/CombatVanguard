using System;

public class DashStateData : StateDataBase
{

    public DashStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        switch (stateType)
        {
            case DashStateData:
                return false;
        }

        return true;
    }

    public override void PlayAnimation(CharacterAnimation characterAnimation)
    {
       characterAnimation.SetDashBool(true);
    }
}