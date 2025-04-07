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

        if (moveDirection != Vector3.zero && !_cameraContllor.RPIsLockOn.CurrentValue)
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
        if (lookDirection != Vector3.zero)
        {
            //�^�[�Q�b�g�̕����������N�I�[�^�j�I��
            Quaternion axisQuaternion = Quaternion.LookRotation(lookDirection);

            //Y���̉�]�p�x�݂̂��擾
            float targetYRotation = axisQuaternion.eulerAngles.y;

            //Y���݂̂̉�]���쐬
            Quaternion lookQuaternion = Quaternion.Euler(0, targetYRotation, 0);

            //�p�x�̓K���iY���̂݁j     
            _characterTransform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                lookQuaternion,
                Time.fixedDeltaTime * _rotationSpeed);
        }
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }
}
