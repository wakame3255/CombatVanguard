using UnityEngine;

public class InterfaceFieldAttribute : PropertyAttribute
{
    public System.Type RequiredType { get; }

    public InterfaceFieldAttribute(System.Type type)
    {
        RequiredType = type;
    }
}
