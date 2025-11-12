using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移を管理するクラス
/// シーンの切り替えやゲームの終了を制御する
/// </summary>
public class SceneChenger : MonoBehaviour
{
    /// <summary>
    /// 指定されたシーンに遷移する
    /// </summary>
    /// <param name="sceneName">遷移先のシーン名</param>
    public void SceneChenge(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// ゲームを終了する
    /// エディター実行時は無効
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
