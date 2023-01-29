using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field, Inherited = false)]
public class FloatRangeSliderAttribute : PropertyAttribute
{
    public float Min { get; private set; }

    public float Max { get; private set; }

    public FloatRangeSliderAttribute(float min, float max)
    {
        if (max < min)
            max = min;

        Min = min;
        Max = max;
    }
}
