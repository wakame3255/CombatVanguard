
public interface IApplicationStateChange
{
    /// <summary>
    /// �X�e�[�g�̃f�[�^�݂̂��擾����
    /// </summary>
    public StateDataInformation StateDataInformation { get; }
    public bool ApplicationStateChange(StateDataBase stateData);
}
