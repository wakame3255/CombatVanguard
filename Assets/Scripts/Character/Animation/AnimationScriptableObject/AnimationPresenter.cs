using R3;

public class AnimationPresenter
{
    private CharacterAnimation _characterAnimation;

    public AnimationPresenter(CharacterStateCont characterStateCont, CharacterAnimation characterAnimation)
    {
        _characterAnimation = characterAnimation;   

        characterStateCont.CurrentStateDataReactiveProperty.Subscribe(state => PlayAnimation(state));
    }

    private void PlayAnimation(StateDataBase stateDataBase)
    {
        stateDataBase.PlayAnimation(_characterAnimation);
    }
}
