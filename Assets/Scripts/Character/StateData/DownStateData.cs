using System;

public class DownStateData : StateDataBase
{
    public DownStateData(ICurrentStateChange currentStateChange)
    {
        _currentStateChange = currentStateChange;
    }

    public override bool CheckChangeState(StateDataBase stateType)
    {
        switch (stateType)
        {
            case DownStateData:
                return false;
        }

        _currentStateChange.ChangeState(stateType);
        return true;
    }

    public override void PlayAnimation(CharacterAnimation characterAnimation)
    {
        characterAnimation.DoHitAnimation();
    }
}