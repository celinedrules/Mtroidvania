using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Abilities))]
public class AbilitiesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        Abilities abilities = (Abilities)target;
        
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Enable All Abilities", GUILayout.Width(EditorGUIUtility.labelWidth - 25));
            abilities.EnableAllAbilities = EditorGUILayout.Toggle(abilities.EnableAllAbilities, GUILayout.MaxWidth(25));
            EditorGUILayout.EndHorizontal();
        }
        
        foreach (FieldInfo field in abilities.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (field.FieldType.IsSubclassOf(typeof(Ability)))
            {
                SerializedProperty abilityProp = serializedObject.FindProperty(field.Name);
                
                if (abilityProp != null)
                    DrawAbility(ObjectNames.NicifyVariableName(field.Name), abilityProp);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawAbility(string label, SerializedProperty abilityProp)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth - 25));

            Ability ability = abilityProp.objectReferenceValue as Ability;
            
            if (ability != null)
            {
                ability.Acquired = EditorGUILayout.Toggle(ability.Acquired, GUILayout.MaxWidth(25));
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.Toggle(false, GUILayout.MaxWidth(25));
                GUI.enabled = true;
            }
            
            EditorGUILayout.PropertyField(abilityProp, GUIContent.none, true);
        }
        EditorGUILayout.EndHorizontal();
    }
}