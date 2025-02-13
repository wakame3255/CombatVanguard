using System;

public class DownStateData : StateDataBase
{
    public DownStateData(ICurrentStateChange currentStateChange)
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
        characterAnimation.DoAnimation(characterAnimation.AnimationData.AttackAnimation.HitAnimation);
    }
}