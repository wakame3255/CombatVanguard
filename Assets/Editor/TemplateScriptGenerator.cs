using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �`���b�gGPT�ɍ���Ă��炢�܂���
/// </summary>
public class TemplateScriptGenerator : EditorWindow
{
    private string[] templateFiles; // �e���v���[�g�t�@�C���̃��X�g
    private int selectedTemplateIndex = 0; // �I�𒆂̃e���v���[�g�C���f�b�N�X
    private string newScriptName = "NewScript"; // �V�����X�N���v�g�̖��O

    [MenuItem("Tools/Template Script Generator")]
    public static void ShowWindow()
    {
        GetWindow<TemplateScriptGenerator>("Template Script Generator");
    }

    private void OnEnable()
    {
        // �e���v���[�g�t�@�C����ǂݍ���
        LoadTemplateFiles();
    }

    private void LoadTemplateFiles()
    {
        // Template�t�H���_�̃p�X���w��
        string templateFolderPath = "Assets/Template";

        // �t�H���_�����݂��邩�m�F
        if (Directory.Exists(templateFolderPath))
        {
            // �w�肵���t�H���_���̑S�Ẵt�@�C�����擾
            string[] files = Directory.GetFiles(templateFolderPath, "*.txt");

            // �t�@�C���������ɕϊ����Ċi�[
            templateFiles = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                templateFiles[i] = Path.GetFileNameWithoutExtension(files[i]);
            }
        }
        else
        {
            Debug.LogError("Template�t�H���_��������܂���: " + templateFolderPath);
            templateFiles = new string[0];
        }
    }

    private void OnGUI()
    {
        // �X�N���v�g�������
        newScriptName = EditorGUILayout.TextField("Script Name", newScriptName);

        // �h���b�v�_�E�����j���[�Ńe���v���[�g��I��
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
            EditorGUILayout.LabelField("Template�t�H���_���Ƀe���v���[�g�t�@�C��������܂���B");
        }
    }

    private void CreateScriptFromTemplate(string templateName, string scriptName)
    {
        // Template�t�@�C���̃p�X
        string templatePath = $"Assets/Template/{templateName}.txt";

        if (File.Exists(templatePath))
        {
            // �e���v���[�g�̓��e��ǂݍ���
            string templateContent = File.ReadAllText(templatePath);

            // #CLASS_NAME# ���X�N���v�g���Œu��������
            templateContent = templateContent.Replace("#SCRIPTNAME#", scriptName);

            // ���ݑI������Ă���t�H���_�̃p�X���擾
            string selectedFolderPath = "Assets"; // �f�t�H���g�� Assets �t�H���_�ɐݒ�
            var selected = Selection.activeObject;
            if (selected != null)
            {
                selectedFolderPath = AssetDatabase.GetAssetPath(selected);

                // �t�H���_���I������Ă��Ȃ��ꍇ�́A�t�@�C�����I������Ă���\�������邽�߁A���̏ꍇ�̓t�H���_�p�X�ɏC��
                if (!Directory.Exists(selectedFolderPath))
                {
                    selectedFolderPath = Path.GetDirectoryName(selectedFolderPath);
                }
            }

            // �V�����X�N���v�g�̕ۑ���Ɩ��O��ݒ�
            string scriptPath = Path.Combine(selectedFolderPath, $"{scriptName}.cs");

            // �t�@�C������������
            File.WriteAllText(scriptPath, templateContent);

            // �A�Z�b�g�f�[�^�x�[�X�����t���b�V�����āA�G�f�B�^�ɐV�����X�N���v�g��F��������
            AssetDatabase.Refresh();

            Debug.Log("�X�N���v�g����������܂���: " + scriptPath);
        }
        else
        {
            Debug.LogError("�e���v���[�g�t�@�C����������܂���: " + templatePath);
        }
    }
}