using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートコントロールを設定するためのインターフェース
/// キャラクターのステート制御を外部から設定可能にする
/// </summary>
public interface ISetStateCont
{
    /// <summary>
    /// ステートコントロールを設定するメソッド
    /// </summary>
    /// <param name="stateCont">ステート制御インターフェース</param>
   public void SetStateCont(IApplicationStateChange stateCont);
}
