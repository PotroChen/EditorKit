using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetProperty
{
    Position = 1 << 0,
    Rotation = 1 << 1,
    Scale = 1 << 2,
    ClearFlags = 1 << 3,
    CullingMask = 1 << 4,
    FieldOfView = 1 << 5,
    NearClipPlane = 1 << 6,
    FarClipPlane = 1 << 7,
    Depth = 1 << 8
}

public enum EnumToIntExample
{
    EnumZero = 0,
    EnumTwo = 1,
    EnumThree = 2,
    EnumFour = 3,
    EnumFive = 4
}

public class Examples : MonoBehaviour
{
    [FloatRangeSlider(0f,10f)]
    public FloatRange floatRangeExample;

    [EnumFlags]
    public TargetProperty enumFlagsExample;

    [EnumToInt(typeof(EnumToIntExample))]
    public int enumToIntExample;

    [FolderPath]
    public string folderPathExample;

    [SortingLayer]
    public int SortingLayerExample;
}
