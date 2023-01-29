using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
[Conditional("UNITY_EDITOR")]
public class SortingLayerAttribute : PropertyAttribute
{

}
