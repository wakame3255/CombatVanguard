using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if UNITY_EDITOR
using UnityEditor;

public class SubClassGenerator : EditorWindow
{
    private string[] _baseClassOptions;
    private string[] _interfaceOptions;
    private int _selectedClassIndex;
    private int _selectedMask;
    private string _newScriptName = "NewClass";

    private HashSet<string> _usings = new();
    [MenuItem("Tools/Sub Class Generator")]
    public static void ShowWindow()
    {
        GetWindow<SubClassGenerator>("Sub Class Generator");
    }

    private void OnEnable()
    {
        LoadAbstractClasses();
        LoadInterfaces();
    }

    private void LoadAbstractClasses()
    {
        Assembly assembly = GetAssemblyCS();

        if (assembly == null)
        {
            Debug.LogWarning("Assembly-CSharp��������܂���ł����B");
            _baseClassOptions = new[] { "None" };
            return;
        }

        _baseClassOptions = assembly.GetTypes()
            .Where(type => type.IsClass && type.IsAbstract && type.IsVisible)
            .Select(type => type.FullName)
            .Prepend("None")
            .ToArray();
    }

    private void LoadInterfaces()
    {
        Assembly assembly = GetAssemblyCS();

        if (assembly == null)
        {
            Debug.LogWarning("Assembly-CSharp��������܂���ł����B");
            _interfaceOptions = Array.Empty<string>();
            return;
        }

        _interfaceOptions = assembly.GetTypes()
            .Where(type => type.IsInterface)
            .Select(type => type.FullName)
            .ToArray();

        _selectedMask = 0;
    }

    private void OnGUI()
    {
        _newScriptName = EditorGUILayout.TextField("Script Name", _newScriptName);

        if (_baseClassOptions.Length > 0)
        {
            _selectedClassIndex = EditorGUILayout.Popup("Select Base Class", _selectedClassIndex, _baseClassOptions);
        }
        else
        {
            EditorGUILayout.LabelField("�v���W�F�N�g���Ɋ��N���X��������܂���ł����B");
        }

        if (_interfaceOptions.Length > 0)
        {
            _selectedMask = EditorGUILayout.MaskField("Select Interface", _selectedMask, _interfaceOptions);
        }
        else
        {
            EditorGUILayout.LabelField("�v���W�F�N�g���ɃC���^�[�t�F�[�X��������܂���ł����B");
        }

        if (GUILayout.Button("Create Script"))
        {
            CreateScript(_baseClassOptions[_selectedClassIndex], _interfaceOptions, _newScriptName);
        }

        if (GUILayout.Button("Reload Classes"))
        {
            LoadAbstractClasses();
            LoadInterfaces();
        }
    }

    private void CreateScript(string baseClassName, string[] interfaceNames, string scriptName)
    {
        _usings.Clear();//_unisgs�̏�����


        List<string> selectedInterfaces = interfaceNames
           .Where((_, index) => (_selectedMask & (1 << index)) != 0)
           .ToList();

        string inheritance = CreateInheritanceSentence(baseClassName, selectedInterfaces.ToArray());
        Assembly assembly = GetAssemblyCS();

        string propertyStub = string.Empty;

        propertyStub += CreateClassPropertyStub(assembly, baseClassName);
        propertyStub += CreateInterfacePropertyStub(assembly, selectedInterfaces.ToArray());

        string methodStubs = string.Empty;

        methodStubs += CreateClassMethodStubs(assembly, baseClassName);
        methodStubs += CreateInterfaceMethodStubs(assembly, selectedInterfaces.ToArray());

        RegisterClassUsings(assembly, baseClassName);
        RegisterInterfaceUsings(assembly, selectedInterfaces.ToArray());

        string usingStatements = string.Empty;
        usingStatements = CreateUsingStatement(_usings);


        string scriptContent = $"using System;\n" +
                               $"{usingStatements}\n" +
                               $"public class {scriptName}{inheritance}\n" +
                               "{\n" +
                               propertyStub +
                               methodStubs +
                               "}";

        string scriptPath = GetGeneratePath(scriptName);

        File.WriteAllText(scriptPath, scriptContent);
        Debug.Log("�X�N���v�g����������܂���: " + scriptPath);

        AssetDatabase.Refresh();
    }


    /// <summary>
    /// �p�������̕��͂𐶐�
    /// </summary>
    /// <param name="baseClassName"></param>
    /// <param name="selectedInterfaces"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string CreateInheritanceSentence(string baseClassName, string[] selectedInterfaces)
    {


        List<string> inheritanceList = new List<string>();

        if (baseClassName != "None")
        {
            inheritanceList.Add(baseClassName);
        }
        inheritanceList.AddRange(selectedInterfaces);

        string inheritance = string.Empty;
        if (inheritanceList.Count > 0)
        {
            inheritance = $" : {string.Join(", ", inheritanceList)}";
        }
        return inheritance;
    }


    #region CreateMethodStub
    /// <summary>
    /// abstractMethod�̕��͂��o��
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="baseClassName"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string CreateClassMethodStubs(Assembly assembly, string baseClassName)
    {
        string sentence = string.Empty;
        if (baseClassName != "None")
        {


            Type baseClassType = assembly?.GetType(baseClassName);

            if (baseClassType != null)
            {
                IEnumerable<string> abstractMethods = baseClassType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(mInfo => mInfo.IsAbstract && !mInfo.IsSpecialName)
                .Select(mInfo => CreateMethodSentence(mInfo, true));

                sentence += string.Join("\n", abstractMethods);
            }
        }
        return sentence;
    }
    /// <summary>
    /// interface�̃��\�b�h�̕��͂��o��
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="selectedInterfaces"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string CreateInterfaceMethodStubs(Assembly assembly, string[] selectedInterfaces)
    {
        string sentence = string.Empty;
        foreach (string interfaceName in selectedInterfaces)
        {


            Type interfaceType = assembly?.GetType(interfaceName);

            if (interfaceType == null)
            {
                continue;
            }
            IEnumerable<string> interfaceMethods = interfaceType.GetMethods()
                    .Where(mInfo => !mInfo.IsSpecialName)
                  .Select(mInfo => CreateMethodSentence(mInfo, false));

            sentence += string.Join("\n", interfaceMethods);
        }
        return sentence;
    }
    #endregion CreateMethodStub
    #region CreatePropertyStub

    /// <summary>
    /// �N���X�̃v���p�e�B���擾
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="baseClassName"></param>
    /// <returns></returns>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string CreateClassPropertyStub(Assembly assembly, string baseClassName)
    {
        string sentence = string.Empty;
        if (baseClassName == "None")
        {
            return string.Empty;
        }
        Type baseClassType = assembly?.GetType(baseClassName);

        if (baseClassType == null)
        {
            return string.Empty;
        }
        // ���ۃv���p�e�B�̎擾�E����
        IEnumerable<string> abstractProperties = baseClassType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(property => property.GetGetMethod(true)?.IsAbstract == true || property.GetSetMethod(true)?.IsAbstract == true)
            .Select(property => CreatePropertyStub(property, true));
        sentence += string.Join("\n", abstractProperties);
        return sentence + "\n";
    }

    /// <summary>
    /// interface�̃v���p�e�B���擾
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="selectedInterfaces"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string CreateInterfacePropertyStub(Assembly assembly, string[] selectedInterfaces)
    {
        string sentence = string.Empty;
        foreach (string interfaceName in selectedInterfaces)
        {


            Type interfaceType = assembly?.GetType(interfaceName);

            if (interfaceType == null)
            {
                continue;
            }
            IEnumerable<string> interfacePropertyStubs = interfaceType.GetProperties()
                 .Where(property => property.GetGetMethod(true)?.IsAbstract == true || property.GetSetMethod(true)?.IsAbstract == true)
                 .Select(property => CreatePropertyStub(property,false));
            sentence += string.Join("\n", interfacePropertyStubs);
        }
        return sentence + "\n";
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string CreatePropertyStub(PropertyInfo property, bool isOverride)
    {
        string overrideSentence=string.Empty;
        if (isOverride)
        {
            overrideSentence = "override";
        }
        string modifier=string.Empty;
        modifier=GetPropertyAccessModifier(property);
        string getPropertySentence = string.Empty;
        bool hasGetter = property.GetGetMethod(true) != null;
        if (hasGetter)
        {
            getPropertySentence = "get => throw new NotImplementedException(); ";
        }

        string setPropertySentence = string.Empty;
        bool hasSetter = property.GetSetMethod(true) != null;
        if (hasSetter)
        {
            setPropertySentence = "set => throw new NotImplementedException(); ";
        }

        return $"    {modifier} {overrideSentence} {ConvertType(property.PropertyType)} {property.Name} {{ " +
               getPropertySentence +
               setPropertySentence +
               "}";
    }
    #endregion CreatePropertyStub
    #region CreateMethodSentence
    /// <summary>
    /// ���\�b�h�̏���Ԃ���
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string CreateMethodSentence(MethodInfo info, bool isOverride)
    {
        string overrideSetence = string.Empty;
        if (isOverride)
        {
            overrideSetence = "override";
        }
        string modifiere = GetMethodAccessModifier(info);
        return $"    {modifiere} {overrideSetence} {ConvertType(info.ReturnType)} {info.Name}({string.Join(", ", info.GetParameters().Select(p => ConvertType(p.ParameterType) + " " + p.Name))})\n    {{\n        throw new NotImplementedException();\n    }}\n";
    }
    /// <summary>
    /// ����ȕϊ��̕K�v������ꍇ,�ϊ����ĕԂ�
    /// </summary>
    /// <param name="convertType"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string ConvertType(Type convertType)
    {
        if (convertType == typeof(void)) return "void";
        if (convertType == typeof(object)) return "object";
        if (convertType == typeof(string)) return "string";
        if (convertType == typeof(UnityEngine.Object)) return "UnityEngine.Object";
        if (convertType == typeof(Int32)) return "int";


        if (convertType.IsGenericType) // �W�F�l���b�N�^�̏ꍇ
        {
            string baseName = convertType.Name.Split('`')[0]; // `List` �̂悤�� `List<T>` ���� `List` �������擾            
            string genericArgs = string.Join(", ", convertType.GetGenericArguments().Select(ConvertType));// �ċA�I�Ɍ^�ϊ�
            return $"{baseName}<{genericArgs}>";
        }

        return convertType.Name;
    }
    #endregion CreateMethodSentence

    /// <summary>
    /// ��������p�X���擾
    /// </summary>
    /// <param name="scriptName"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string GetGeneratePath(string scriptName)
    {
        string selectedFolderPath = "Assets";
        Object selected = Selection.activeObject;

        if (selected != null)
        {
            selectedFolderPath = AssetDatabase.GetAssetPath(selected);
            if (!Directory.Exists(selectedFolderPath))
            {
                selectedFolderPath = Path.GetDirectoryName(selectedFolderPath);
            }
        }

        string scriptPath = Path.Combine(selectedFolderPath, $"{scriptName}.cs");
        return scriptPath;
    }

    /// <summary>
    /// Assembly-CSharp��Assembly��Ԃ�
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Assembly GetAssemblyCS()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Assembly-CSharp");
    }
    #region getModifier
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string GetMethodAccessModifier(MethodInfo method)
    {
        if (method.IsPublic)
        {
            return "public";
        }
        if (method.IsPrivate)
        {
            return "private";
        }
        if (method.IsFamily) // protected
        {
            return "protected";
        }
        if (method.IsAssembly) // internal
        {
            return "internal";
        }
        if (method.IsFamilyOrAssembly) // protected internal
        {
            return "protected internal";
        }
        if (method.IsFamilyAndAssembly) // private protected
        {
            return "private protected";
        }

        return "public"; // �f�t�H���g�� public�i�ʏ�A�����ɂ͓��B���Ȃ��j
    }

    /// <summary>
    /// �v���p�e�B�S�̂̃A�N�Z�X�C���q������
    /// </summary>
    private string GetPropertyAccessModifier(PropertyInfo prop)
    {
        MethodInfo getter = prop.GetGetMethod(true);
        MethodInfo setter = prop.GetSetMethod(true);
        bool hasGetter = getter!=null;
        string getAccess = string.Empty;
        if(hasGetter)
        {
            getAccess = GetMethodAccessModifier(getter);
        }
        bool hasSetter = setter != null;
        string setAccess = string.Empty;
        if (hasSetter)
        {
            setAccess = GetMethodAccessModifier(setter);
        }
        

        // �ł��L���A�N�Z�X�C���q��K�p
        return GetWidestAccessModifier(getAccess, setAccess);
    }

    /// <summary>
    /// �ł��L���A�N�Z�X�C���q������
    /// </summary>
    static string GetWidestAccessModifier(string access1, string access2)
    {
        string[] order = { "private", "private protected", "protected", "internal", "protected internal", "public" };

        string widest = "private"; // �f�t�H���g�� private
        foreach (string level in order)
        {
            if (access1 == level || access2 == level)
            {
                widest = level;
            }
        }
        return widest;
    }
    #endregion getModifier
    #region usings
    private void RegisterClassUsings(Assembly assembly, string baseClassName)
    {
        string sentence = string.Empty;
        if (baseClassName == "None")
        {
            return;
        }
        Type baseClassType = assembly?.GetType(baseClassName);
        RegisterUsingSentence(baseClassType);
        if (baseClassType == null)
        {
            return;
        }
        IEnumerable<PropertyInfo> properties = baseClassType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                                        .Where(property => property.GetGetMethod(true)?.IsAbstract == true || property.GetSetMethod(true)?.IsAbstract == true);


        foreach (PropertyInfo property in properties)
        {
            RegisterPropertyUsings(property);
        }
        IEnumerable<MethodInfo> methods = baseClassType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                            .Where(mInfo => mInfo.IsAbstract && !mInfo.IsSpecialName);

        foreach (MethodInfo method in methods)
        {
            RegisterMethodUsings(method);
        }
    }
    private void RegisterInterfaceUsings(Assembly assembly, string[] selectInterfaces)
    {
        foreach (string interfaceName in selectInterfaces)
        {
            Type interfaceType = assembly?.GetType(interfaceName);

            if (interfaceType == null)
            {
                continue;
            }
            foreach (PropertyInfo property in interfaceType.GetProperties())
            {
                RegisterPropertyUsings(property);
            }
            foreach (MethodInfo method in interfaceType.GetMethods())
            {
                RegisterMethodUsings(method);
            }
        }

    }



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RegisterPropertyUsings(PropertyInfo info)
    {
        RegisterUsingSentence(info.PropertyType);
    }

    private void RegisterMethodUsings(MethodInfo info)
    {
        RegisterUsingSentence(info.ReturnType);
        foreach (ParameterInfo parameter in info.GetParameters())
        {
            RegisterUsingSentence(parameter.ParameterType);
        }
    }

    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    private void RegisterUsingSentence(Type type)
    {
        if (type.Namespace == null)
        {
            return;
        }
        if (type.Namespace == "System")
        {
            return;
        }
        _usings.Add($"using {type.Namespace};");
    }
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    private string CreateUsingStatement(HashSet<string> usings)
    {
        string sentence = string.Empty;
        sentence = string.Join("\n", _usings);
        return sentence;
    }
    #endregion

}
#endif