using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定されたゲームオブジェクトを複数生成するクラス
/// 開始時に設定された数だけオブジェクトをインスタンス化する
/// </summary>
public class InstanceObj : MonoBehaviour
{
    /// <summary>
    /// インスタンス化するゲームオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject _instanceObj;

    /// <summary>
    /// インスタンス化する数
    /// </summary>
    [SerializeField]
    private int _instanceCount;

    /// <summary>
    /// 開始時に呼ばれる初期化処理
    /// 指定された数だけオブジェクトを生成する
    /// </summary>
    private void Start()
    {
        // 指定された回数分、オブジェクトをインスタンス化
        for (int i = 0; i < _instanceCount; i++)
        {
            Instantiate(_instanceObj, this.transform);
        }
    }
}
