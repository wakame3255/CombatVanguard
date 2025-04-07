using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement; // EditorSceneManager�̂��߂ɕK�v
using UnityEngine.SceneManagement; // Scene�Ǘ��̋@�\�̂��߂ɕK�v
using System;

// �G�f�B�^�[�g���̃N���X
public class AutoSaveConfig : ScriptableObject
{
    public bool isEnabled = true;
    public float saveInterval = 30f;
    public bool saveOnlyModified = true; // �ύX���������ꍇ�̂ݕۑ�����ݒ��ǉ�
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

        // �V�[�����ύX����Ă��邩�m�F
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
            Debug.Log($"[AutoSave] �v���W�F�N�g�������ۑ����܂���: {DateTime.Now}");
        }
    }
}

// �ݒ�p�̃G�f�B�^�[�E�B���h�E
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

        if (GUILayout.Button("�ݒ�̕ۑ�"))
        {
            config = ScriptableObject.CreateInstance<AutoSaveConfig>();
            Debug.Log("�ۑ�");
        }
    }
}
