using System;
using UnityEngine;

public static class AnimationStringUtility
{
   
    private static string _moveInputXName = "InputX";
    
    private static string _moveInputYName = "InputY";
   
    private static string _isDashName = "IsDash";
   
    private static string _isGuard = "IsGuard";

    public static string MoveInputXName { get => _moveInputXName; }
    public static string MoveInputYName { get => _moveInputYName; }
    public static string IsDashName { get => _isDashName; }
    public static string IsGuardName { get => _isGuard; }
} 