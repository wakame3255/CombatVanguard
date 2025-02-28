
using UnityEngine;

public abstract class StateDataBase
{
   protected ICurrentStateChange _currentStateChange;

    /// <summary>
    /// ���̃X�e�[�g�ɑJ�ڂł��邩�`�F�b�N����
    /// </summary>
    /// <param name="stateType">���ɑJ�ڂ������m�[�h</param>
    /// <returns>�\���ǂ���</returns>
    public abstract bool CheckChangeState(StateDataBase stateType);
    public abstract AnimationClip PlayAnimation(CharacterAnimation characterAnimation);
}
