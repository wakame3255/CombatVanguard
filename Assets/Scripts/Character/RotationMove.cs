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

        //ターゲットの方向を向くクオータニオン
        Quaternion axisQuaternion = Quaternion.LookRotation(lookDirection);

        //向きたい角度
        Vector3 targetRotation = axisQuaternion.eulerAngles;

        //角度の変更
        lookQuaternion = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);

        //角度の適応      
        _characterTransform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuaternion, Time.fixedDeltaTime * _rotationSpeed);
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }
}
