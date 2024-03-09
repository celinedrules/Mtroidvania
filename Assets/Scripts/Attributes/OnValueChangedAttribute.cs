using UnityEngine;

public class OnValueChangedAttribute : PropertyAttribute
{
    public string MethodName { get; private set; }
    public bool ExecuteInPlayModeOnly { get; private set; }

    public OnValueChangedAttribute(string methodName, bool executeInPlayModeOnly = false)
    {
        MethodName = methodName;
        ExecuteInPlayModeOnly = executeInPlayModeOnly;
    }
}
