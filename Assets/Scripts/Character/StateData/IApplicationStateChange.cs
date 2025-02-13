
using R3;

public interface IApplicationStateChange
{
    /// <summary>
    /// ステートのデータのみを取得する
    /// </summary>
    public StateDataInformation StateDataInformation { get; }
    public StateDataBase CurrentStateData { get;}
    public bool ApplicationStateChange(StateDataBase stateData);
    public void UpdateDebug();
    public void CheckMoveState(bool isDash, bool isGuard);
}
