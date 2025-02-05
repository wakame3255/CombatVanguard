using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement; // EditorSceneManagerのために必要
using UnityEngine.SceneManagement; // Scene管理の機能のために必要
using System;

// エディター拡張のクラス
public class AutoSaveConfig : ScriptableObject
{
    public bool isEnabled = true;
    public float saveInterval = 60f; // 5分（秒単位）
}

[InitializeOnLoad]
public class AutoSave
{
    private static AutoSaveConfig config;
    private static DateTime lastSaveTime;

    // コンストラクタ（エディター起動時に実行）
    static AutoSave()
    {
        // 設定の読み込み
        config = ScriptableObject.CreateInstance<AutoSaveConfig>();
        EditorApplication.update += Update;
        lastSaveTime = DateTime.Now;
    }

    static void Update()
    {
        if (!config.isEnabled)
            return;

        // 設定された間隔で保存を実行
        if ((DateTime.Now - lastSaveTime).TotalSeconds >= config.saveInterval)
        {
            SaveAll();
            lastSaveTime = DateTime.Now;
        }
    }

    // シーンとアセットの保存を実行
    static void SaveAll()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
            Debug.Log($"[AutoSave] プロジェクトを自動保存しました: {DateTime.Now}");
        } 
    }
}

// 設定用のエディターウィンドウ
public class AutoSaveSettingsWindow : EditorWindow
{
    private static AutoSaveConfig config;

    [MenuItem("Tools/Auto Save/Settings")]
    public static void ShowWindow()
    {
        GetWindow<AutoSaveSettingsWindow>("Auto Save Settings");
    }

    void OnGUI()
    {
        if (config == null)
            config = ScriptableObject.CreateInstance<AutoSaveConfig>();

        config.isEnabled = EditorGUILayout.Toggle("Auto Save Enabled", config.isEnabled);
        config.saveInterval = EditorGUILayout.FloatField("Save Interval (seconds)", config.saveInterval);

        if (GUILayout.Button("設定の保存"))
        {
            config = ScriptableObject.CreateInstance<AutoSaveConfig>();
            Debug.Log("保存");
        }
    }
}
