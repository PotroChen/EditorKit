using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Inspector中显示Enum，储存的是Int
/// 警告：只对Enum.Values的直都是Int的枚举有用
/// </summary>
[CustomPropertyDrawer(typeof(EnumToIntAttribute))]
public class EnumToIntDrawer: PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumToIntAttribute enumToIntAttribute = (EnumToIntAttribute)attribute;

        if (enumToIntAttribute == null|| enumToIntAttribute.enumType == null)
            return;

        System.Type type = enumToIntAttribute.enumType;

        var values = System.Enum.GetValues(type);

        int lastIndex = 0;
        for (int i = 0; i < values.Length; i++)
        {
            if (property.intValue == (int)values.GetValue(i))
            {
                lastIndex = i;
                break;
            }
        }
        var currentIndex = EditorGUI.Popup(position,label.text, lastIndex, System.Enum.GetNames(type));
        property.intValue = (int)values.GetValue(currentIndex);
    }
}
 