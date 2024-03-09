using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(OnValueChangedAttribute))]
public class OnValueChangedDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(position, property, label);
        
        OnValueChangedAttribute onValueChangedAttribute = (OnValueChangedAttribute)attribute;

        if (!onValueChangedAttribute.ExecuteInPlayModeOnly ||
            (onValueChangedAttribute.ExecuteInPlayModeOnly && EditorApplication.isPlaying))
        {
            object target = property.serializedObject.targetObject;
            MethodInfo method = target.GetType().GetMethod(onValueChangedAttribute.MethodName, 
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            
            if (method != null)
                method.Invoke(target, null);
            else
                Debug.LogWarning($"Method named '{onValueChangedAttribute.MethodName}' not found on {target.GetType()}");
        }
    }

    // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    // {
    //     EditorGUI.BeginChangeCheck();
    //     EditorGUI.PropertyField(position, property, label);
    //
    //     if (EditorGUI.EndChangeCheck())
    //     {
    //         // Apply the modified properties to update the serialized object
    //         property.serializedObject.ApplyModifiedProperties();
    //
    //         // Now invoke the method, as the serialized object is updated
    //         OnValueChangedAttribute onValueChangedAttribute = (OnValueChangedAttribute)this.attribute;
    //         object target = property.serializedObject.targetObject;
    //         MethodInfo method = target.GetType().GetMethod(onValueChangedAttribute.MethodName, 
    //             BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    //
    //         if (method != null)
    //             method.Invoke(target, null);
    //         else
    //             Debug.LogWarning($"Method named '{onValueChangedAttribute.MethodName}' not found on {target.GetType()}");
    //     }
    // }
}
