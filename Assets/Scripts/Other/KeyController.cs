using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class KeyController : MonoBehaviour
{
    public static KeyController Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public static Dictionary<Key, KeyCode> keyDict = new Dictionary<Key, KeyCode>()
    {
        {Key.up,KeyCode.W},{Key.down,KeyCode.S},{Key.left,KeyCode.A},{Key.right,KeyCode.D}
    };

    public static void ChangeKey(Key key,KeyCode code)
    {
        if (keyDict.ContainsKey(key)) keyDict[key] = code;
        else keyDict.Add(key, code);
    }

    private static float H, V;
    //private static float Sensitive = 5;
    //private static float Damping = 5f;
    private void Update()
    {
        if(Input.GetKey(keyDict[Key.up]) && !Input.GetKey(keyDict[Key.down])) 
        {

        }
    }
}

public enum Key
{
    up,down,left,right,
}*/
