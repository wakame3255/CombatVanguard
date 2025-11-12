
/// <summary>
/// デバッグ用ユーティリティクラス
/// ログ出力などのデバッグ機能を提供する
/// </summary>
public static class DebugUtility
{
    /// <summary>
    /// デバッグログを出力する
    /// </summary>
    /// <param name="message">出力するメッセージ</param>
   public static void Log(string message)
    {
        UnityEngine.Debug.Log(message);
    }
}
