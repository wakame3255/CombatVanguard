using System.ComponentModel;
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// C# 9.0のinit専用セッターをサポートするための内部クラス
    /// .NET 5未満の環境でinit専用セッターを使用可能にする
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IsExternalInit
    {
    }
}
