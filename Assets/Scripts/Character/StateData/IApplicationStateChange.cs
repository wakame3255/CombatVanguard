
public interface IApplicationStateChange
{
    /// <summary>
    /// ステートのデータのみを取得する
    /// </summary>
    public StateDataInformation StateDataInformation { get; }
    public bool ApplicationStateChange(StateDataBase stateData);
}
