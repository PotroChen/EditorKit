using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatRangeSliderAttribute))]
public class FloatRangeSliderDrawer :PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int originalIndentLevel = EditorGUI.indentLevel;

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(
            position, GUIUtility.GetControlID(FocusType.Passive), label
        );
        EditorGUI.indentLevel = 0;

        SerializedProperty minProperty = property.FindPropertyRelative("min");
        SerializedProperty maxProperty = property.FindPropertyRelative("max");

        float minValue = minProperty.floatValue;
        float maxValue = maxProperty.floatValue;
        //设置数值填写处和滑动条的宽度
        float fieldWidth = position.width / 4f - 4f;
        float sliderWidth = position.width / 2f;
        //创建minValue的数据填写处
        position.width = fieldWidth;
        minValue = EditorGUI.FloatField(position, minValue);
        //创建滑动条
        position.x += fieldWidth+4f;
        position.width = sliderWidth;
        FloatRangeSliderAttribute limit = attribute as FloatRangeSliderAttribute;
        EditorGUI.MinMaxSlider(
            position, ref minValue, ref maxValue, limit.Min, limit.Max
        );
        //创建maxValue的数据填写处
        position.x += sliderWidth + 4f;
        position.width = fieldWidth;
        maxValue = EditorGUI.FloatField(position, maxValue);
        //限制大小
        if (minValue < limit.Min)
        {
            minValue = limit.Min;
        }
        if (maxValue < minValue)
        {
            maxValue = minValue;
        }
        else if (maxValue > limit.Max)
        {
            maxValue = limit.Max;
        }

        minProperty.floatValue = minValue;
        maxProperty.floatValue = maxValue;

        EditorGUI.EndProperty();

        EditorGUI.indentLevel = originalIndentLevel;
    }
}
