using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSingleTon<T> where T : CSSingleTon<T>
{
    static volatile T _uniqueInstance;

    protected CSSingleTon()
    {

    }
    public static T _instance
    {
        get
        {
            if (_uniqueInstance == null)
            {
                lock (typeof(T))
                {
                    if (_uniqueInstance == null)
                    {
                        _uniqueInstance = System.Activator.CreateInstance(typeof(T)) as T;
                    }
                }
            }
            return _uniqueInstance;
        }
    }
}
