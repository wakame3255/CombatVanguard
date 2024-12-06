using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Transform _characterTransform;

    /// <summary>
    /// �������\�b�h
    /// </summary>
    /// <param name="moveDirection">�ړ��������</param>
    public void DoMove(Vector3 moveDirection)
    {
        _characterTransform.position += moveDirection.normalized* _speed * Time.deltaTime; 
    }

    /// <summary>
    /// �g�����X�t�H�[�������炤���\�b�h
    /// </summary>
    /// <param name="characterTransform">�L�����N�^�[�̃g�����X�t�H�[��</param>
    public void SetTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }
}
