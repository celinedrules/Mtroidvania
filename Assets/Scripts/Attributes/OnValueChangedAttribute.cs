using UnityEngine;

public class OnValueChangedAttribute : PropertyAttribute
{
    public string MethodName { get; private set; }

    public OnValueChangedAttribute(string methodName)
    {
        MethodName = methodName;
    }
}
