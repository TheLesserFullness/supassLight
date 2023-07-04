using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    private static T _singleton;

    static Singleton()
    {
        _singleton = new T();
        
    }

    public static T singleton
    {
        get
        {
            if (_singleton == null)
            {
                _singleton = new T();
            }
            
            return _singleton;
        }
    }

    public virtual bool Init()
    {
        return false;
    }

    public virtual void UnInit()
    {

    }

    public virtual void OnLogOut()
    {
        _singleton = default(T);
    }


    /*
        static Singleton()
        {
            _Instance = new Singleton();
        }

        //private static object _SyncObj = new object();
        public static Singleton Instance
        {
            get { return _Instance; }
        }*/

}
