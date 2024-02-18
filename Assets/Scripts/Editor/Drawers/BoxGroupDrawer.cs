using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BoxGroupAttribute))]
public class BoxGroupDrawer : PropertyDrawer
{
    private const float VerticalSpaceBeforeGroup = 15f; // Space before the group box
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        BoxGroupAttribute boxGroup = attribute as BoxGroupAttribute;
        string groupName = boxGroup.groupName;
        string propertyLabel = label.text;
        float labelPadding = 5.0f;

        // if (boxGroup.index == 0)
        //     position.y += VerticalSpaceBeforeGroup;
        
        float groupHeight = GetGroupHeight(property.serializedObject, boxGroup.groupName);
        
        // Calculate the height needed for the group label if this is the first item in the group.
        float groupLabelHeight = boxGroup.index == 0 ? EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing : 0;

        if (boxGroup.index == 0)
        {
            position.y += VerticalSpaceBeforeGroup;

            // Calculate the border size and adjust the box size to include the border
            float borderWidth = 1f; // Width of the border
            Rect borderRect = new Rect(position.x - EditorGUIUtility.standardVerticalSpacing - borderWidth, 
                position.y - EditorGUIUtility.standardVerticalSpacing - borderWidth, 
                position.width + EditorGUIUtility.standardVerticalSpacing * 2 + borderWidth * 2, 
                groupHeight + EditorGUIUtility.standardVerticalSpacing * 2 + borderWidth * 2);

            // Draw the border rectangle
            EditorGUI.DrawRect(borderRect, new Color(0f, 0f, 0f, 1f)); // Dark grey, solid color for the border

            // Draw the inner box (the original rectangle)
            Rect boxRect = new Rect(position.x - EditorGUIUtility.standardVerticalSpacing, 
                position.y - EditorGUIUtility.standardVerticalSpacing, 
                position.width + EditorGUIUtility.standardVerticalSpacing * 2, 
                groupHeight + EditorGUIUtility.standardVerticalSpacing * 2);
            EditorGUI.DrawRect(boxRect, new Color(.23f, .23f, .23f, 1f)); // Light grey, semi-transparent for the inner box

            // Box around the group name
            GUIStyle groupNameStyle = EditorStyles.boldLabel;
            Vector2 groupNameSize = groupNameStyle.CalcSize(new GUIContent(groupName));
            Rect groupNameBorderRect = new Rect(position.x - borderWidth, 
                position.y - EditorGUIUtility.standardVerticalSpacing, 
                position.width + borderWidth * 2, 
                groupNameSize.y + borderWidth * 2 + 5);
            EditorGUI.DrawRect(groupNameBorderRect, new Color(0.3f, 0.3f, 0.3f, 1f)); // Group name border color
            
            Rect bottomBorderRect = new Rect(groupNameBorderRect.x, 
                groupNameBorderRect.yMax - borderWidth, // Positioned at the bottom of groupNameBorderRect
                groupNameBorderRect.width, 
                borderWidth);
            EditorGUI.DrawRect(bottomBorderRect, new Color(0f, 0f, 0f, 1f)); // Adjust color as needed
        }
        
        // Adjust the position for the group label and draw it if this is the first item in the group.
        if (groupLabelHeight > 0)
        {
            Rect labelPosition = new Rect(position.x + labelPadding, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelPosition, groupName, EditorStyles.boldLabel);
            position.y += groupLabelHeight; // Move down after header to create space for the property.
        }

        label.text = propertyLabel;

        //position.y += 5;
        
        // Adjust position for the property field to ensure it's drawn at the correct Y position.
        Rect propertyPosition = new Rect(position.x + labelPadding, position.y, position.width, EditorGUI.GetPropertyHeight(property, label, true));
        
        // Handle prefab overrides and other metadata for the property.
        EditorGUI.BeginProperty(propertyPosition, label, property);

        // Draw the property field using the correct label.
        EditorGUI.PropertyField(propertyPosition, property, label, true);

        // Finish handling metadata.
        EditorGUI.EndProperty();
    }

    // Override GetPropertyHeight to provide custom spacing.
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        BoxGroupAttribute boxGroup = attribute as BoxGroupAttribute;
        // Calculate extra height for the group label for the first item.
        float extraHeight = boxGroup.index == 0 ? EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing : 0;

        if (boxGroup.index == 0)
            extraHeight += VerticalSpaceBeforeGroup;
        
        // Return the total height needed (property height + extra height for group label).
        return base.GetPropertyHeight(property, label) + extraHeight;
    }
    
    private float GetGroupHeight(SerializedObject serializedObject, string groupName)
    {
        float height = 0;
        Type objectType = serializedObject.targetObject.GetType();
        FieldInfo[] fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            var attributes = field.GetCustomAttributes(typeof(BoxGroupAttribute), true);
            foreach (BoxGroupAttribute attr in attributes)
            {
                if (attr.groupName == groupName)
                {
                    // Assuming a single-line height for simplicity; adjust as needed.
                    height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    break; // Found the attribute, no need to check further.
                }
            }
        }

        // Optionally, add some extra space for padding or headers as needed.
        return height += EditorGUIUtility.singleLineHeight;// + EditorGUIUtility.standardVerticalSpacing;
    }
}
