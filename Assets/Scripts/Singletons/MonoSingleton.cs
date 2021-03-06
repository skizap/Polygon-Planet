﻿using UnityEngine;
using System.Collections;

/*
 * CLASS MonoSingleton
 * -------------------
 * A type of singleton class that inherits from monobehaviour
 * Classes inheriting from this class should have a function
 * call this function with the [RuntimeInitializeOnLoadMethod]
 * attribute applied to it
 * -------------------
 */

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T instance { get { return _instance; } }

    protected static void BaseCreateInstance()
    {
        BaseCreateInstance("MonoSingleton");
    }

    protected static void BaseCreateInstance(string objName)
    {
        if(instance == null)
        {
            GameObject parentObj = new GameObject(objName);
            _instance = parentObj.AddComponent<T>();
            DontDestroyOnLoad(parentObj);
        }
    }
}
