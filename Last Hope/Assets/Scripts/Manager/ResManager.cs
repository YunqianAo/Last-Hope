using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : Singleton<ResManager>
{
    public GameObject LoadUI(string path)
    {
        GameObject go = Resources.Load<GameObject>($"UIPrefab/{path}");
        if ( go == null)
        {
            Debug.LogError($"UI Window not found{path}"); return null;
        }
        GameObject obj= GameObject.Instantiate( go );
        return obj;
    }
}
