using System;

public class GuardStateData : StateDataBase
{
    public GuardStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        switch (stateType)
        {
            case GuardStateData:
                return false;
        }
        return true;
    }

    public override void PlayAnimation(CharacterAnimation characterAnimation)
    {
        characterAnimation.SetAnimationBool(AnimationStringUtility.IsGuardName);
    }
}