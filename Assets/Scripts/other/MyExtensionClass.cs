
using UnityEngine;
using UnityEngine.DedicatedServer;

/// <summary>
/// 拡張メソッドを提供する静的クラス
/// MonoBehaviourに便利な機能を追加する
/// </summary>
public static class MyExtensionClass 
{
    /// <summary>
    /// コンポーネントの存在確認。なかった場合はAddを行う
    /// </summary>
    /// <typeparam name="T">チェックを行うコンポーネント型</typeparam>
    /// <param name="monoBehaviour">この拡張メソッドを呼び出すMonoBehaviour</param>
    /// <param name="gameObject">対象のGameObject（nullの場合は自身）</param>
    /// <returns">取得または追加されたコンポーネント</returns>
    public static T CheckComponentMissing<T>(this MonoBehaviour monoBehaviour, GameObject gameObject = null) where T : class
    {
        T component;

        if (gameObject == null)
        {
            component = SetComponent<T>(monoBehaviour.gameObject);
        }
        else
        {
            component = SetComponent<T>(gameObject);
        }

        return component;
    }

    /// <summary>
    /// 引数がnullかどうかをチェックするメソッド
    /// nullの場合は例外をスローする
    /// </summary>
    /// <typeparam name="T">引数の型</typeparam>
    /// <param name="arugment">チェック対象の引数</param>
    /// <param name="arugmentName">引数名</param>
    /// <exception cref="System.ArgumentNullException">引数がnullの場合</exception>
    public static void CheckArgumentNull<T>(T arugment, string arugmentName)
    {
        if (arugment == null)
        {
            throw new System.ArgumentNullException(arugmentName);
        }
    }

    /// <summary>
    /// コンポーネントを取得または追加する内部メソッド
    /// </summary>
    /// <typeparam name="T">コンポーネント型</typeparam>
    /// <param name="gameObject">対象のGameObject</param>
    /// <returns>取得または追加されたコンポーネント</returns>
    private static T SetComponent<T>(GameObject gameObject) where T : class
    {
        T component;

        if(!gameObject.TryGetComponent<T>(out component))
        {
            Debug.LogError(gameObject.transform.name + " " + typeof(T).FullName + "がありません");
            component = gameObject.AddComponent(typeof(T)) as T;
        }

        return component;
    }
}
