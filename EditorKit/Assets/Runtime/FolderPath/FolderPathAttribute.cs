using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
public class FolderPathAttribute : PropertyAttribute
{
	public string defaultPath = "";

	public FolderPathAttribute()
	{
		
	}

	public FolderPathAttribute(string defaultPath)
	{
		this.defaultPath = defaultPath;
	}
}
