using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationMove : MonoBehaviour, ISetTransform
{
    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private CameraContllor _cameraContllor;

    private Transform _characterTransform;

   public void CheckRotation(Vector3 moveDirection)
    {
        if (_cameraContllor == null)
        {
            DoRotation(moveDirection);
            return;
        }

        if (moveDirection == Vector3.zero && !_cameraContllor.RPIsLockOn.CurrentValue)
        {
            DoRotation(moveDirection);
        }
        else if(_cameraContllor.RPIsLockOn.CurrentValue)
        {
            DoRotation(_cameraContllor.CameraTransform.forward);
        }  
    }

    private void DoRotation(Vector3 lookDirection)
    {
        Quaternion lookQuaternion;

        //�^�[�Q�b�g�̕����������N�I�[�^�j�I��
        Quaternion axisQuaternion = Quaternion.LookRotation(lookDirection);

        //���������p�x
        Vector3 targetRotation = axisQuaternion.eulerAngles;

        //�p�x�̕ύX
        lookQuaternion = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);

        //�p�x�̓K��      
        _characterTransform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuaternion, Time.fixedDeltaTime * _rotationSpeed);
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }
}
