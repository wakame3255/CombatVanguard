

public interface ICurrentStateChange
{
    /// <summary>
    /// �X�e�[�g�̃f�[�^�݂̂��擾����
    /// </summary>
    public StateDataInformation StateDataInformation { get;}


    /// <summary>
    /// �X�e�[�g�̕ύX���s��
    /// </summary>
    /// <param name="stateData">�ύX�w��</param>
    public void ChangeState(StateJudgeBase stateData);
}
