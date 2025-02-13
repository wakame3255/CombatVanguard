
using R3;

public interface IApplicationStateChange
{
    /// <summary>
    /// �X�e�[�g�̃f�[�^�݂̂��擾����
    /// </summary>
    public StateDataInformation StateDataInformation { get; }
    public StateDataBase CurrentStateData { get;}
    public bool ApplicationStateChange(StateDataBase stateData);
    public void UpdateDebug();
    public void CheckMoveState(bool isDash, bool isGuard);
}
