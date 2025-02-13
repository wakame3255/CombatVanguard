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

        _currentStateChange.ChangeState(stateType);
        return true;
    }

    public override void PlayAnimation(CharacterAnimation characterAnimation)
    {
       characterAnimation.SetAnimationBool(AnimationStringUtility.IsDashName);
    }
}