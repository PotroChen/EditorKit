using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumToIntAttribute : PropertyAttribute
{
    public Type enumType;

    public EnumToIntAttribute(Type enumType)
    {
        this.enumType = enumType;
    }
}
