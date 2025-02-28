
using UnityEngine;

/// <summary>
/// �X�e�[�g�f�[�^�̊��N���X
/// </summary>
public abstract class StateJudgeBase
{
   protected ICurrentStateChange _currentStateChange;

    /// <summary>
    /// ���̃X�e�[�g�ɑJ�ڂł��邩�`�F�b�N����
    /// </summary>
    /// <param name="stateType">���ɑJ�ڂ������m�[�h</param>
    /// <returns>�\���ǂ���</returns>
    public abstract bool CheckChangeState(StateJudgeBase stateType);
}
