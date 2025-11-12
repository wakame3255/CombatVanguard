using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// キャラクターの回転移動を制御するクラス
/// カメラの向きとロックオン状態を考慮して回転を行う
/// </summary>
public class RotationMove : MonoBehaviour, ISetTransform
{
    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField]
    private float _rotationSpeed;

    /// <summary>
    /// カメラコントローラー
    /// </summary>
    [SerializeField]
    private CameraContllor _cameraContllor;

    /// <summary>
    /// キャラクターのTransform
    /// </summary>
    private Transform _characterTransform;

    /// <summary>
    /// 回転をチェックして実行する
    /// ロックオン状態に応じて回転方向を決定する
    /// </summary>
    /// <param name="moveDirection">移動方向</param>
   public void CheckRotation(Vector3 moveDirection)
    {
        // カメラコントローラーが設定されていない場合は移動方向に回転
        if (_cameraContllor == null)
        {
            DoRotation(moveDirection);
            return;
        }

        // ロックオンしていない場合は移動方向に回転
        if (moveDirection != Vector3.zero && !_cameraContllor.RPIsLockOn.CurrentValue)
        {
            DoRotation(moveDirection);
        }
        // ロックオン中はカメラの前方向に回転
        else if(_cameraContllor.RPIsLockOn.CurrentValue)
        {
            DoRotation(_cameraContllor.CameraTransform.forward);
        }  
    }

    /// <summary>
    /// 指定された方向に回転する
    /// Y軸のみの回転を滑らかに適用する
    /// </summary>
    /// <param name="lookDirection">向く方向</param>
    private void DoRotation(Vector3 lookDirection)
    {
        if (lookDirection != Vector3.zero)
        {
            // ターゲットの方向からクォータニオンを作成
            Quaternion axisQuaternion = Quaternion.LookRotation(lookDirection);

            // Y軸の回転角度のみを取得
            float targetYRotation = axisQuaternion.eulerAngles.y;

            // Y軸のみの回転を作成
            Quaternion lookQuaternion = Quaternion.Euler(0, targetYRotation, 0);

            // 角度の適用（Y軸のみ）     
            _characterTransform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                lookQuaternion,
                Time.fixedDeltaTime * _rotationSpeed);
        }
    }

    /// <summary>
    /// キャラクターのTransformを設定する
    /// </summary>
    /// <param name="characterTransform">キャラクターのTransform</param>
    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }
}
