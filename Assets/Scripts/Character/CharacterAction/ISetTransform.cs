using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISetTransform
{
    /// <summary>
    /// �g�����X�t�H�[�������炤���\�b�h
    /// </summary>
    /// <param name="characterTransform">�L�����N�^�[�̃g�����X�t�H�[��</param>
    public void SetCharacterTransform(Transform characterTransform);
}
