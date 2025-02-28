

public interface ICurrentStateChange
{
    /// <summary>
    /// ステートのデータのみを取得する
    /// </summary>
    public StateDataInformation StateDataInformation { get;}


    /// <summary>
    /// ステートの変更を行う
    /// </summary>
    /// <param name="stateData">変更指定</param>
    public void ChangeState(StateJudgeBase stateData);
}
