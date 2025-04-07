using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement; // EditorSceneManagerのために必要
using UnityEngine.SceneManagement; // Scene管理の機能のために必要
using System;

// エディター拡張のクラス
public class AutoSaveConfig : ScriptableObject
{
    public bool isEnabled = true;
    public float saveInterval = 30f;
    public bool saveOnlyModified = true; // 変更があった場合のみ保存する設定を追加
}

[InitializeOnLoad]
public class AutoSave
{
    private static AutoSaveConfig config;
    private static DateTime lastSaveTime;

    static AutoSave()
    {
        config = ScriptableObject.CreateInstance<AutoSaveConfig>();
        EditorApplication.update += Update;
        lastSaveTime = DateTime.Now;
    }

    static void Update()
    {
        if (!config.isEnabled)
            return;

        if ((DateTime.Now - lastSaveTime).TotalSeconds >= config.saveInterval)
        {
            SaveAll();
            lastSaveTime = DateTime.Now;
        }
    }

    static void SaveAll()
    {
        if (EditorApplication.isPlaying)
            return;

        bool shouldSave = !config.saveOnlyModified;

        // シーンが変更されているか確認
        if (config.saveOnlyModified)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isDirty)
                {
                    shouldSave = true;
                    break;
                }
            }
        }

        if (shouldSave)
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
        config.saveOnlyModified = EditorGUILayout.Toggle("Save Only Modified Scenes", config.saveOnlyModified);

        if (GUILayout.Button("設定の保存"))
        {
            config = ScriptableObject.CreateInstance<AutoSaveConfig>();
            Debug.Log("保存");
        }
    }
}
