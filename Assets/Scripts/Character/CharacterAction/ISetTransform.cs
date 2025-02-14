using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISetTransform
{
    /// <summary>
    /// トランスフォームをもらうメソッド
    /// </summary>
    /// <param name="characterTransform">キャラクターのトランスフォーム</param>
    public void SetCharacterTransform(Transform characterTransform);
}
