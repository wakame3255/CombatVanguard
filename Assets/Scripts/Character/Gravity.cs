using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 重力を適用するクラス
/// キャラクターに下向きの力を適用して重力をシミュレートする
/// </summary>
public class Gravity : MonoBehaviour
{
    /// <summary>
    /// 重力の強さ
    /// </summary>
    [SerializeField]
    private float _gravityForce;

    /// <summary>
    /// 物理演算の更新処理
    /// 重力を適用する
    /// </summary>
    private void FixedUpdate()
    {
        AdaptationGravity();
    }
 
    /// <summary>
    /// 重力を適用する
    /// オブジェクトを下方向に移動させる
    /// </summary>
    public void AdaptationGravity()
    {
        transform.position += Vector3.down * _gravityForce * Time.deltaTime;
    }
}
