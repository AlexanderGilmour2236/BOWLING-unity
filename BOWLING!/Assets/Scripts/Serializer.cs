using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class Serializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<BallSkin> ballSkins = new List<BallSkin>();
        
        BallSkin b = new BallSkin();
        b.Color = Color.blue;
        b.Name = "Игорь";

        BallSkin a = new BallSkin();
        a.Name = "Вася";
        b.Color = Color.red;
        
        ballSkins.Add(b);
        ballSkins.Add(a);
        
        var jsonArray = new JSONArray();

        foreach (var skin in ballSkins)
        {
            var jsonSkin = new JSONClass();
            jsonSkin["Name"] = skin.Name;
            var jsonColor = new JSONClass();
            jsonColor.Add("r", new JSONData(skin.Color.r)); 
            jsonColor.Add("g", new JSONData(skin.Color.g));
            jsonColor.Add("b", new JSONData(skin.Color.b));
            jsonColor.Add("a", new JSONData(skin.Color.a));

            jsonSkin["Color"] = jsonColor;
            
            jsonArray.Add(jsonSkin);
        }

        Debug.Log(jsonArray.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
