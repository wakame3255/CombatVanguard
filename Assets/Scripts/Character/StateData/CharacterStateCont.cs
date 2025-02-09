using R3;
using System.Diagnostics;

public class CharacterStateCont : ICurrentStateChange, IApplicationStateChange
{
    public StateDataBase CurrentStateData { get; private set; }
    public StateDataInformation StateDataInformation { get; private set; } 

    public ReactiveProperty<StateDataBase> CurrentStateDataReactiveProperty { get; private set; }


    public CharacterStateCont()
    {
        CurrentStateDataReactiveProperty = new ReactiveProperty<StateDataBase>();
        StateDataInformation = new StateDataInformation(this);
        CurrentStateDataReactiveProperty.Value = StateDataInformation.NormalStateData;
        CurrentStateData = CurrentStateDataReactiveProperty.Value;
    }

    /// <summary>
    /// ステートの変更依頼のみを行う
    /// </summary>
    /// <param name="stateData"></param>
    public bool ApplicationStateChange(StateDataBase stateData)
    {
        return CurrentStateDataReactiveProperty.Value.CheckChangeState(stateData);
    }

    /// <summary>
    /// ステートの変更を行う
    /// </summary>
    /// <param name="stateData"></param>
    public void ChangeState(StateDataBase stateData)
    {
        CurrentStateData = stateData;
        CurrentStateDataReactiveProperty.Value = stateData;
    }

    public void UpdateDebug()
    {
        UnityEngine.Debug.Log(CurrentStateData);
    }
}


public class StateDataInformation
{
    public AvoidanceStateData AvoidanceStateData { get; private set; }
    public DownStateData DownStateData { get; private set; }
    public AttackStateData AttackStateData { get; private set; }
    public GuardStateData GuardStateData { get; private set; }
    public NormalStateData NormalStateData { get; private set; }

    public StateDataInformation(ICurrentStateChange currentStateChange)
    {
        NormalStateData = new NormalStateData(currentStateChange);
        GuardStateData = new GuardStateData(currentStateChange);
        AttackStateData = new AttackStateData(currentStateChange);
        DownStateData = new DownStateData(currentStateChange);
        AvoidanceStateData = new AvoidanceStateData(currentStateChange);
    }
}
