using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// チャットGPTに作ってもらいました
/// </summary>
public class TemplateScriptGenerator : EditorWindow
{
    private string[] templateFiles; // テンプレートファイルのリスト
    private int selectedTemplateIndex = 0; // 選択中のテンプレートインデックス
    private string newScriptName = "NewScript"; // 新しいスクリプトの名前

    [MenuItem("Tools/Template Script Generator")]
    public static void ShowWindow()
    {
        GetWindow<TemplateScriptGenerator>("Template Script Generator");
    }

    private void OnEnable()
    {
        // テンプレートファイルを読み込む
        LoadTemplateFiles();
    }

    private void LoadTemplateFiles()
    {
        // Templateフォルダのパスを指定
        string templateFolderPath = "Assets/Template";

        // フォルダが存在するか確認
        if (Directory.Exists(templateFolderPath))
        {
            // 指定したフォルダ内の全てのファイルを取得
            string[] files = Directory.GetFiles(templateFolderPath, "*.txt");

            // ファイル名だけに変換して格納
            templateFiles = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                templateFiles[i] = Path.GetFileNameWithoutExtension(files[i]);
            }
        }
        else
        {
            Debug.LogError("Templateフォルダが見つかりません: " + templateFolderPath);
            templateFiles = new string[0];
        }
    }

    private void OnGUI()
    {
        // スクリプト名を入力
        newScriptName = EditorGUILayout.TextField("Script Name", newScriptName);

        // ドロップダウンメニューでテンプレートを選択
        if (templateFiles.Length > 0)
        {
            selectedTemplateIndex = EditorGUILayout.Popup("Select Template", selectedTemplateIndex, templateFiles);

            if (GUILayout.Button("Create Script"))
            {
                CreateScriptFromTemplate(templateFiles[selectedTemplateIndex], newScriptName);
            }
        }
        else
        {
            EditorGUILayout.LabelField("Templateフォルダ内にテンプレートファイルがありません。");
        }
    }

    private void CreateScriptFromTemplate(string templateName, string scriptName)
    {
        // Templateファイルのパス
        string templatePath = $"Assets/Template/{templateName}.txt";

        if (File.Exists(templatePath))
        {
            // テンプレートの内容を読み込む
            string templateContent = File.ReadAllText(templatePath);

            // #CLASS_NAME# をスクリプト名で置き換える
            templateContent = templateContent.Replace("#SCRIPTNAME#", scriptName);

            // 現在選択されているフォルダのパスを取得
            string selectedFolderPath = "Assets"; // デフォルトは Assets フォルダに設定
            var selected = Selection.activeObject;
            if (selected != null)
            {
                selectedFolderPath = AssetDatabase.GetAssetPath(selected);

                // フォルダが選択されていない場合は、ファイルが選択されている可能性があるため、その場合はフォルダパスに修正
                if (!Directory.Exists(selectedFolderPath))
                {
                    selectedFolderPath = Path.GetDirectoryName(selectedFolderPath);
                }
            }

            // 新しいスクリプトの保存先と名前を設定
            string scriptPath = Path.Combine(selectedFolderPath, $"{scriptName}.cs");

            // ファイルを書き込む
            File.WriteAllText(scriptPath, templateContent);

            // アセットデータベースをリフレッシュして、エディタに新しいスクリプトを認識させる
            AssetDatabase.Refresh();

            Debug.Log("スクリプトが生成されました: " + scriptPath);
        }
        else
        {
            Debug.LogError("テンプレートファイルが見つかりません: " + templatePath);
        }
    }
}