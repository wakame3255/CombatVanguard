using UnityEngine;

/// <summary>
/// インターフェース型のフィールドをInspectorで編集可能にする属性
/// Unity Editorでインターフェースを実装したコンポーネントを選択できるようにする
/// </summary>
public class InterfaceFieldAttribute : PropertyAttribute
{
    /// <summary>
    /// 必要とされる型
    /// </summary>
    public System.Type RequiredType { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="type">インターフェース型</param>
    public InterfaceFieldAttribute(System.Type type)
    {
        RequiredType = type;
    }
}
