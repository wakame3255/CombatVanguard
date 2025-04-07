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
            //ターゲットの方向を向くクオータニオン
            Quaternion axisQuaternion = Quaternion.LookRotation(lookDirection);

            //Y軸の回転角度のみを取得
            float targetYRotation = axisQuaternion.eulerAngles.y;

            //Y軸のみの回転を作成
            Quaternion lookQuaternion = Quaternion.Euler(0, targetYRotation, 0);

            //角度の適応（Y軸のみ）     
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
