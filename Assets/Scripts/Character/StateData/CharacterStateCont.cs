using R3;
using System.Diagnostics;

/// <summary>
/// キャラクターのステートを管理するクラス
/// ステート遷移とアニメーション再生を制御する
/// </summary>
public class CharacterStateCont : ICurrentStateChange, IApplicationStateChange
{
    /// <summary>
    /// キャラクターアニメーション判定クラス
    /// </summary>
    private CharacterAnimationJudge _characterAnimationJuge;

    /// <summary>
    /// 現在のステートデータ
    /// </summary>
    public StateJudgeBase CurrentStateData { get; private set; }

    /// <summary>
    /// ステートデータ情報
    /// </summary>
    public StateDataInformation StateDataInformation { get; private set; } 

    /// <summary>
    /// 現在のステートデータのReactiveProperty
    /// </summary>
    public ReactiveProperty<StateJudgeBase> CurrentStateDataReactiveProperty { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// 初期状態を通常ステートに設定する
    /// </summary>
    /// <param name="characterAnimation">キャラクターアニメーション</param>
    public CharacterStateCont(CharacterAnimation characterAnimation)
    {
        CurrentStateDataReactiveProperty = new ReactiveProperty<StateJudgeBase>();
        StateDataInformation = new StateDataInformation(this);
        CurrentStateDataReactiveProperty.Value = StateDataInformation.NormalStateData;
        CurrentStateData = CurrentStateDataReactiveProperty.Value;

        _characterAnimationJuge = new CharacterAnimationJudge(characterAnimation);
    }

    /// <summary>
    /// ステートの変更依頼のみを行う
    /// 実際の変更は現在のステートの判定による
    /// </summary>
    /// <param name="stateData">遷移先のステートデータ</param>
    /// <returns>遷移が成功したかどうか</returns>
    public bool ApplicationStateChange(StateJudgeBase stateData)
    {
        return CurrentStateDataReactiveProperty.Value.CheckChangeState(stateData);
    }

    /// <summary>
    /// ステートの変更を行う
    /// 新しいステートに遷移し、対応するアニメーションを再生する
    /// </summary>
    /// <param name="stateData">遷移先のステートデータ</param>
    public void ChangeState(StateJudgeBase stateData)
    {
        CurrentStateData = stateData;
        CurrentStateDataReactiveProperty.Value = stateData;
        _characterAnimationJuge.JudgePlayAnimation(stateData);
    }

    /// <summary>
    /// ステートデバッグ用
    /// 現在のステートをログ出力する
    /// </summary>
    public void UpdateDebug()
    {
        //DebugUtility.Log(CurrentStateData.ToString());
    }

    /// <summary>
    /// 移動ステートをチェックして適切なステートに遷移する
    /// ガード、ダッシュ、歩行の優先順位で判定
    /// </summary>
    /// <param name="isDash">ダッシュ中かどうか</param>
    /// <param name="isGuard">ガード中かどうか</param>
    public void CheckMoveState(bool isDash, bool isGuard)
    {
        if (isGuard)
        {
            ApplicationStateChange(StateDataInformation.GuardStateData);
        }
        else if (isDash)
        {
            ApplicationStateChange(StateDataInformation.DashStateData);
        }
        else
        {
            ApplicationStateChange(StateDataInformation.WalkStateData);
        }
    }
}

/// <summary>
/// 全てのステートデータを保持する情報クラス
/// 各種ステート判定クラスのインスタンスを管理する
/// </summary>
public class StateDataInformation
{
    /// <summary>回避ステートデータ</summary>
    public AvoidanceStateJudge AvoidanceStateData { get; private set; }

    /// <summary>ダウンステートデータ</summary>
    public DownStateJudge DownStateData { get; private set; }

    /// <summary>攻撃ステートデータ</summary>
    public AttackStateJudge AttackStateData { get; private set; }

    /// <summary>ガードステートデータ</summary>
    public GuardStateJudge GuardStateData { get; private set; }

    /// <summary>ガード被弾ステートデータ</summary>
    public GuardHitStateJudge GuardHitStateData { get; private set; }

    /// <summary>歩行ステートデータ</summary>
    public WalkStateJudge WalkStateData { get; private set; }

    /// <summary>ダッシュステートデータ</summary>
    public DashStateJudge DashStateData { get; private set; }

    /// <summary>パリィステートデータ</summary>
    public ParryStateJudge ParryStateData { get; private set; }

    /// <summary>パリィ被弾ステートデータ</summary>
    public HitParryStateJudge HitParryStateData { get; private set; }

    /// <summary>通常ステートデータ</summary>
    public NormalStateJudge NormalStateData { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// 全てのステート判定クラスのインスタンスを生成する
    /// </summary>
    /// <param name="currentStateChange">ステート変更インターフェース</param>
    public StateDataInformation(ICurrentStateChange currentStateChange)
    {
        NormalStateData = new NormalStateJudge(currentStateChange);
        WalkStateData = new WalkStateJudge(currentStateChange);
        DashStateData = new DashStateJudge(currentStateChange);
        GuardStateData = new GuardStateJudge(currentStateChange);
        GuardHitStateData = new GuardHitStateJudge(currentStateChange);
        AttackStateData = new AttackStateJudge(currentStateChange);
        DownStateData = new DownStateJudge(currentStateChange);
        AvoidanceStateData = new AvoidanceStateJudge(currentStateChange);
        ParryStateData = new ParryStateJudge(currentStateChange);
        HitParryStateData = new HitParryStateJudge(currentStateChange);
    }
}
