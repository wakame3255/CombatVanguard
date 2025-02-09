using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePosition : MonoBehaviour
{
    [SerializeField]
    private float radius = 5.0f; // 円の半径

    [SerializeField]
    private float speed = 1.0f; // 移動速度

    private float angle = 0.0f; // 現在の角度

    // Update is called once per frame
    void Update()
    {
        // 角度を更新
        angle += speed * Time.deltaTime;

        // 新しい位置を計算
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // オブジェクトの位置を更新
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
