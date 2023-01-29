using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
public class EnumToIntAttribute : PropertyAttribute
{
    public Type enumType;

    public EnumToIntAttribute(Type enumType)
    {
        this.enumType = enumType;
    }
}
