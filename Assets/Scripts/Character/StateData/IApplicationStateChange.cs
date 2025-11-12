using R3;

/// <summary>
/// ステート変更を適用するためのインターフェース
/// ステートデータの取得と変更を提供する
/// </summary>
public interface IApplicationStateChange
{
    /// <summary>
    /// ステートのデータのみを取得する
    /// </summary>
    public StateDataInformation StateDataInformation { get; }

    /// <summary>
    /// 現在のステートデータ
    /// </summary>
    public StateJudgeBase CurrentStateData { get;}

    /// <summary>
    /// 現在のステートデータのReactiveProperty
    /// </summary>
    public ReactiveProperty<StateJudgeBase> CurrentStateDataReactiveProperty { get; }

    /// <summary>
    /// ステート変更を適用する
    /// </summary>
    /// <param name="stateData">変更先のステートデータ</param>
    /// <returns>変更が成功したかどうか</returns>
    public bool ApplicationStateChange(StateJudgeBase stateData);

    /// <summary>
    /// デバッグ情報を更新する
    /// </summary>
    public void UpdateDebug();

    /// <summary>
    /// 移動ステートをチェックする
    /// </summary>
    /// <param name="isDash">ダッシュ中かどうか</param>
    /// <param name="isGuard">ガード中かどうか</param>
    public void CheckMoveState(bool isDash, bool isGuard);
}
