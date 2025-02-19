
using R3;

public interface IApplicationStateChange
{
    /// <summary>
    /// ステートのデータのみを取得する
    /// </summary>
    public StateDataInformation StateDataInformation { get; }
    public StateJudgeBase CurrentStateData { get;}
    public ReactiveProperty<StateJudgeBase> CurrentStateDataReactiveProperty { get; }
    public bool ApplicationStateChange(StateJudgeBase stateData);
    public void UpdateDebug();
    public void CheckMoveState(bool isDash, bool isGuard);
}
