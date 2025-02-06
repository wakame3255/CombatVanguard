
public abstract class StateDataBase
{
   protected ICurrentStateChange _currentStateChange;
   public abstract bool CheckChangeState(StateDataBase stateType);
    public abstract void PlayAnimation(CharacterAnimation characterAnimation);
}
