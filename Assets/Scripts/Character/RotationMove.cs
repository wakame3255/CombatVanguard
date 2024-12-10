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

        //ターゲットの方向を向くクオータニオン
        Quaternion axisQuaternion = Quaternion.LookRotation(moveDirection);

        //向きたい角度
        Vector3 targetRotation = axisQuaternion.eulerAngles;

        //角度の変更
        lookQuaternion = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);

        //角度の適応      
        _characterTransform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuaternion, Time.fixedDeltaTime* _rotationSpeed);
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }
}
