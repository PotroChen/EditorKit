using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatRangeExample : MonoBehaviour
{
    [FloatRangeSlider(0f, 100f)]
    public FloatRange value;
}
