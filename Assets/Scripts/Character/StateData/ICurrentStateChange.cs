
/// <summary>
/// 現在のステート変更を行うためのインターフェース
/// </summary>
public interface ICurrentStateChange
{
    /// <summary>
    /// ステートのデータのみを取得する
    /// </summary>
    public StateDataInformation StateDataInformation { get;}

    /// <summary>
    /// ステートの変更を行う
    /// </summary>
    /// <param name="stateData">変更先のステートデータ</param>
    public void ChangeState(StateJudgeBase stateData);
}
