using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトを円形軌道上で移動させるクラス
/// 中心点を原点として円を描くように移動する
/// </summary>
public class MovePosition : MonoBehaviour
{
    /// <summary>
    /// 円の半径
    /// </summary>
    [SerializeField]
    private float radius = 5.0f;

    /// <summary>
    /// 移動速度（ラジアン/秒）
    /// </summary>
    [SerializeField]
    private float speed = 1.0f;

    /// <summary>
    /// 現在の角度（ラジアン）
    /// </summary>
    private float angle = 0.0f;

    /// <summary>
    /// 毎フレーム呼ばれる更新処理
    /// 円形軌道上の位置を計算して更新する
    /// </summary>
    void Update()
    {
        // 角度を更新
        angle += speed * Time.deltaTime;

        // 新しい位置を計算
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // オブジェクトの位置を更新（Y座標は維持）
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
