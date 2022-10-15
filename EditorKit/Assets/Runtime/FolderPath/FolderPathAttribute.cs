using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
