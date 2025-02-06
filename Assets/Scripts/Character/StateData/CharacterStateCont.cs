using R3;
using System.Diagnostics;

public class CharacterStateCont : ICurrentStateChange, IApplicationStateChange
{
    public StateDataInformation StateDataInformation { get; private set; } 

    public ReactiveProperty<StateDataBase> CurrentStateDataReactiveProperty { get; private set; }


    public CharacterStateCont()
    {
        CurrentStateDataReactiveProperty = new ReactiveProperty<StateDataBase>();
        StateDataInformation = new StateDataInformation(this);
        CurrentStateDataReactiveProperty.Value = StateDataInformation.NormalStateData; 
    }

    /// <summary>
    /// �X�e�[�g�̕ύX�˗��݂̂��s��
    /// </summary>
    /// <param name="stateData"></param>
    public bool ApplicationStateChange(StateDataBase stateData)
    {
        Debug.WriteLine("ApplicationStateChange");
        return CurrentStateDataReactiveProperty.Value.CheckChangeState(stateData);
    }

    /// <summary>
    /// �X�e�[�g�̕ύX���s��
    /// </summary>
    /// <param name="stateData"></param>
    public void ChangeState(StateDataBase stateData)
    {
        UnityEngine.Debug.Log(stateData);
        CurrentStateDataReactiveProperty.Value = stateData;
    }
}


public class StateDataInformation
{
    public AttackStateData AttackStateData { get; private set; }
    public GuardStateData GuardStateData { get; private set; }
    public NormalStateData NormalStateData { get; private set; }

    public StateDataInformation(ICurrentStateChange currentStateChange)
    {
        NormalStateData = new NormalStateData(currentStateChange);
        GuardStateData = new GuardStateData(currentStateChange);
        AttackStateData = new AttackStateData(currentStateChange);
    }
}
