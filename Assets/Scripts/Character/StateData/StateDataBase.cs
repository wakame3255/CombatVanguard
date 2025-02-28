
using UnityEngine;

public abstract class StateDataBase
{
   protected ICurrentStateChange _currentStateChange;

    /// <summary>
    /// 次のステートに遷移できるかチェックする
    /// </summary>
    /// <param name="stateType">次に遷移したいノード</param>
    /// <returns>可能かどうか</returns>
    public abstract bool CheckChangeState(StateDataBase stateType);
    public abstract AnimationClip PlayAnimation(CharacterAnimation characterAnimation);
}
