using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transformを設定するためのインターフェース
/// キャラクターのTransformを外部から設定可能にする
/// </summary>
public interface ISetTransform
{
    /// <summary>
    /// キャラクターのTransformを設定するメソッド
    /// </summary>
    /// <param name="characterTransform">キャラクターのTransform</param>
    public void SetCharacterTransform(Transform characterTransform);
}
