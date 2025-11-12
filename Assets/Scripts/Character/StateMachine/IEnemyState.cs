using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のステートインターフェース
/// 全ての敵のステートが実装するべきメソッドを定義する
/// </summary>
public interface IEnemyState
{
    /// <summary>
    /// ステートに入るときの処理
    /// </summary>
    public void EnterState();

    /// <summary>
    /// ステートの更新処理（毎フレーム呼ばれる）
    /// </summary>
    public void UpdateState();

    /// <summary>
    /// ステートから出るときの処理
    /// </summary>
    public void ExitState();
}