using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InterfaceFieldAttribute))]
public class InterfaceFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        InterfaceFieldAttribute attribute = (InterfaceFieldAttribute)base.attribute;

        Object currentObject = property.objectReferenceValue;

        EditorGUI.BeginProperty(position, label, property);

        Object newObject = EditorGUI.ObjectField(position, label, currentObject, typeof(Object), true);

        if (newObject is GameObject gameObject)
        {
            // GameObjectから必要なコンポーネントを取得
            var component = gameObject.GetComponent(attribute.RequiredType);
            if (component == null)
            {
                Debug.LogWarning($"The GameObject does not contain a component implementing {attribute.RequiredType.Name}.");
                newObject = null;
            }
            else
            {
                newObject = component; // 必要なコンポーネントを割り当て
            }
        }
        else if (newObject is Component component)
        {
            if (!attribute.RequiredType.IsAssignableFrom(component.GetType()))
            {
                Debug.LogWarning($"The Component does not implement {attribute.RequiredType.Name}. Actual type: {component.GetType().Name}");
                newObject = null;
            }
        }
        else if (newObject != null)
        {
            Debug.LogWarning($"Assigned object is not valid. Actual type: {newObject?.GetType().Name}");
            newObject = null;
        }

        property.objectReferenceValue = newObject;

        EditorGUI.EndProperty();
    }
}
