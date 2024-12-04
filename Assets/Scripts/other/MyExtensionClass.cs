
using UnityEngine;
using UnityEngine.DedicatedServer;

public static class MyExtensionClass 
{
    /// <summary>
    /// �R���|�[�l���g���݊m�F�B�Ȃ������ꍇ��Add���s��
    /// </summary>
    /// <typeparam name="T">�`�F�b�N�̍s���R���|�[�l���g</typeparam>
    /// <param name="monoBehaviour">�g�����\�b�h���Ăяo��MonoBehaviour</param>
    /// <returns>�R���|�[�l���g</returns>
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
    /// ������null���ǂ������`�F�b�N���郁�\�b�h
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arugment">����</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static void CheckArgumentNull<T>(T arugment, string arugmentName)
    {
        if (arugment == null)
        {
            throw new System.ArgumentNullException(arugmentName);
        }
    }

    private static T SetComponent<T>(GameObject gameObject) where T : class
    {
        T component;

        if(!gameObject.TryGetComponent<T>(out component))
        {
            Debug.LogError(gameObject.transform.name + " " + typeof(T).FullName + "������Ȃ���");
            component = gameObject.AddComponent(typeof(T)) as T;
        }

        return component;
    }
}
