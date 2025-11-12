using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションコンポーネントを設定するためのインターフェース
/// </summary>
public interface ISetAnimation 
{
    /// <summary>
    /// キャラクターアニメーションコンポーネントを設定する
    /// </summary>
    /// <param name="characterAnimation">キャラクターアニメーション</param>
    public void SetAnimationComponent(CharacterAnimation characterAnimation);
}
