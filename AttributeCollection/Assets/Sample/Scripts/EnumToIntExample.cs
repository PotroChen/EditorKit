using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumToIntExample : MonoBehaviour
{
    public enum TestEnum
    {
        Apple = 0,
        Banana = 2,
        Orange = 4
    }


    [EnumToInt(typeof(TestEnum))]
    public int value = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
