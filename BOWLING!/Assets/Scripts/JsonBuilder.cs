using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class JsonBuilder : MonoBehaviour
{
    #region Singleton

    private static JsonBuilder _instance;

    public static JsonBuilder Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject ins = new GameObject("JSONBuilder");
                ins.AddComponent<JsonBuilder>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    #endregion
    
    public static string ToJson<T>(List<T> list)
    {
        StringBuilder str = new StringBuilder();

        str.Append("[");
        int count = 0;

        foreach (T obj in list)
        {
            count++;
            str.Append(JsonUtility.ToJson(obj));

            if (count != list.Count)
            {
                str.Append(",");
            }
        }
        str.Append("]");

        return str.ToString();
    }
}
