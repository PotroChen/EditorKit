using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System;

[CustomPropertyDrawer(typeof(SortingLayerAttribute))]
public class SortingLayerAttributeDrawer : PropertyDrawer
{
    List<int> iDs = new List<int>();
    List<string> contents = new List<string>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SortingLayer[] sortingLayers = SortingLayer.layers;
        if (sortingLayers.Length <= 0)
        {
            property.intValue = EditorGUI.IntField(position, label, property.intValue);
            return;
        }

        iDs.Clear();
        contents.Clear();
        for (int i = 0; i < sortingLayers.Length; i++)
        {
            SortingLayer sortingLayer = sortingLayers[i];

            if (string.IsNullOrEmpty(sortingLayer.name.Trim()))
                continue;

            iDs.Add(i);
            contents.Add(sortingLayer.name);
        }
        int selectedIndex = 0;
        for (int i = 0; i < iDs.Count; i++)
        {
            if (property.intValue == iDs[i])
            {
                selectedIndex = i;
                break;
            }
        }
        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, contents.ToArray());
        property.intValue = iDs[selectedIndex];
    }
}
