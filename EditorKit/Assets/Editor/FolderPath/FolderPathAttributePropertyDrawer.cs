using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(FolderPathAttribute))]
public class FolderPathAttributePropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		float browserBtnWidth = Mathf.Min(25f, position.width * 0.1f);
		float leftWidth = position.width - browserBtnWidth;
		Rect leftRect = position;
		leftRect.width = leftWidth;

		Rect rightWidth = position;
		rightWidth.x += leftWidth;
		rightWidth.width = browserBtnWidth;

		EditorGUI.PropertyField(leftRect, property, label);
		if(GUI.Button(rightWidth, "..."))
		{
			FolderPathAttribute folderPathAttribute = attribute as FolderPathAttribute;

			string folder = string.IsNullOrEmpty(property.stringValue) ? folderPathAttribute.defaultPath : property.stringValue;
			folder = EditorUtility.SaveFolderPanel("Select a folder", folder, "");
			if(!string.IsNullOrEmpty(folder))
				property.stringValue = folder;

			property.serializedObject.ApplyModifiedProperties();//由于调用了GUIUtility.ExitGUI，后面序列化操作不会进行，手动执行一下。
			GUIUtility.ExitGUI();//终止后面其他的GUI逻辑，不然调用EditorUtility.SaveFolderPanel会导致unity抛异常
		}
		
	}
}