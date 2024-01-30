using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pair<T1, T2>
{
    public T1 Key { get { return _key; } set { _key = value; } }
    public T2 Value { get { return _value; } set { _value = value; } }

    [SerializeField] private T1 _key;
    [SerializeField] private T2 _value;

}
