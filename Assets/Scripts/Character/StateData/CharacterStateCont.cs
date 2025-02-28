using R3;
using System.Diagnostics;

/// <summary>
/// キャラクターのステートを管理するクラス
/// </summary>
public class CharacterStateCont : ICurrentStateChange, IApplicationStateChange
{
    private CharacterAnimationJudge _characterAnimationJuge;

    public StateJudgeBase CurrentStateData { get; private set; }
    public StateDataInformation StateDataInformation { get; private set; } 

    public ReactiveProperty<StateJudgeBase> CurrentStateDataReactiveProperty { get; private set; }


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
    /// </summary>
    /// <param name="stateData"></param>
    public bool ApplicationStateChange(StateJudgeBase stateData)
    {
        return CurrentStateDataReactiveProperty.Value.CheckChangeState(stateData);
    }

    /// <summary>
    /// ステートの変更を行う
    /// </summary>
    /// <param name="stateData"></param>
    public void ChangeState(StateJudgeBase stateData)
    {
        CurrentStateData = stateData;
        CurrentStateDataReactiveProperty.Value = stateData;
        _characterAnimationJuge.JudgePlayAnimation(stateData);
    }

    /// <summary>
    /// ステートデバッグ用
    /// </summary>
    public void UpdateDebug()
    {
        DebugUtility.Log(CurrentStateData.ToString());
    }

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


public class StateDataInformation
{
    public AvoidanceStateJudge AvoidanceStateData { get; private set; }
    public DownStateJudge DownStateData { get; private set; }
    public AttackStateJudge AttackStateData { get; private set; }
    public GuardStateJudge GuardStateData { get; private set; }
    public GuardHitStateJudge GuardHitStateData { get; private set; }
    public WalkStateJudge WalkStateData { get; private set; }
    public DashStateJudge DashStateData { get; private set; }
    public ParryStateJudge ParryStateData { get; private set; }
    public HitParryStateJudge HitParryStateData { get; private set; }
    public NormalStateJudge NormalStateData { get; private set; }

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
