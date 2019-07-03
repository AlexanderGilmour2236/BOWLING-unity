using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = System.Object;

public class DBController : MonoBehaviour
{
    #region Singleton

    private static DBController _instance;

    public static DBController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject ins = new GameObject("DBController");
                ins.AddComponent<DBController>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    #endregion

    #region Events

    public delegate void onGetUser(Player player);
    public static event onGetUser OnGetUser;

    public delegate void onError(Exception ex);
    public static event onError OnError;
    
    #endregion
    
    public string server;

    private void OnDestroy()
    {
        OnError = delegate(Exception ex) {  };
        OnGetUser = delegate(Player player) {  };
    }

    public void GetStats(string playerName)
    {
        StartCoroutine(getStats(playerName));
    }

    public void UpdatePlayers(List<Player> players)
    {
        
        StartCoroutine(updatePlayers(ToJsonBuider(players)));
    }

    private string ToJsonBuider(List<Player> list)
    {
        StringBuilder str = new StringBuilder();

        str.Append("[");
        int count = 0;

        foreach (Player player in list)
        {
            count++;
            str.Append(JsonUtility.ToJson(player));

            if (count != list.Count)
            {
                str.Append(",");
            }
        }
        str.Append("]");

        return str.ToString();
    }
    
    private IEnumerator updatePlayers(string json)
    {
        Debug.Log(json);
        
        WWWForm form = new WWWForm();
        form.AddField("action", "update_stats");
        form.AddField("json", json);
        WWW www = new WWW(server,form);

        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error.ToString());
            yield break;
        }
        Debug.Log(www.text);
    }
    
    private IEnumerator getStats(string playerName)
    {
        WWWForm form = new WWWForm();
        form.AddField("action", "get_stats");
        form.AddField("player_name", playerName);
        WWW www = new WWW(server,form);

        yield return www;
        if (www.error != null)
        {
            if (OnError != null)
            {
                OnError(new Exception(www.error));
            }
            yield break;
        }
        try
        {
            Player p = JsonUtility.FromJson<Player>(www.text);
            if (OnGetUser != null)
            {
                OnGetUser(p);
            }
        }
        catch(Exception ex)
        {
            if (OnGetUser != null)
            {
                OnGetUser(null);
            }
        }

    }
}
