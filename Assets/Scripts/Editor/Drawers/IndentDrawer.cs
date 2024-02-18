using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(IndentAttribute))]
public class IndentDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var indentAttribute = attribute as IndentAttribute;
        if (indentAttribute == null)
            return;
        
        EditorGUI.indentLevel += indentAttribute.indentLevel;
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.indentLevel -= indentAttribute.indentLevel;
    }
}