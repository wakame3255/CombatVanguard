using System;
using UnityEngine;

/// <summary>
/// アニメーションパラメータ名を管理する静的ユーティリティクラス
/// Animatorのパラメータ名を一元管理する
/// </summary>
public static class AnimationStringUtility
{
    /// <summary>
    /// X軸移動入力パラメータ名
    /// </summary>
    private static string _moveInputXName = "InputX";
    
    /// <summary>
    /// Y軸移動入力パラメータ名
    /// </summary>
    private static string _moveInputYName = "InputY";
   
    /// <summary>
    /// ダッシュ状態パラメータ名
    /// </summary>
    private static string _isDashName = "IsDash";
   
    /// <summary>
    /// ガード状態パラメータ名
    /// </summary>
    private static string _isGuard = "IsGuard";

    /// <summary>X軸移動入力パラメータ名のプロパティ</summary>
    public static string MoveInputXName { get => _moveInputXName; }

    /// <summary>Y軸移動入力パラメータ名のプロパティ</summary>
    public static string MoveInputYName { get => _moveInputYName; }

    /// <summary>ダッシュ状態パラメータ名のプロパティ</summary>
    public static string IsDashName { get => _isDashName; }

    /// <summary>ガード状態パラメータ名のプロパティ</summary>
    public static string IsGuardName { get => _isGuard; }
}
