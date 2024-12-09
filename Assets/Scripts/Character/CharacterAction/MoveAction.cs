using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour, ISetTransform
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
   
    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }
}
