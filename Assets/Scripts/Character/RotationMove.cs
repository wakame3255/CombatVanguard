using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMove : MonoBehaviour, ISetTransform
{
    [SerializeField]
    private float _rotationSpeed;

    private Transform _characterTransform;

   public void DoRotation(Vector3 moveDirection)
    {
        if (moveDirection == Vector3.zero)
        {
            return;
        }

        Quaternion lookQuaternion;

        //�^�[�Q�b�g�̕����������N�I�[�^�j�I��
        Quaternion axisQuaternion = Quaternion.LookRotation(moveDirection);

        //���������p�x
        Vector3 targetRotation = axisQuaternion.eulerAngles;

        //�p�x�̕ύX
        lookQuaternion = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);

        //�p�x�̓K��      
        _characterTransform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuaternion, Time.fixedDeltaTime* _rotationSpeed);
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }
}
