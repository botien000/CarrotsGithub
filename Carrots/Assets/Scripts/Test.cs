using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;
public class Test : MonoBehaviour
{
    public Text textt;
    private string path;
    int a = 0;
    private void Start()
    {
        //GetDataAllMaps(5);
        //List<Level> a = new List<Level>() {new Level(),new Level()};
        //for (int i = 0; i < a.Count; i++)
        //{
        //    a[i].score = i;
        //}
        //var e = from level in a
        //        orderby level.score descending
        //        select level;
        //foreach (var item in e)
        //{
        //    Debug.Log(item.score);
        //}
        //MapData map = new MapData(1);
        //map.SetScore(2, 3);
        //string path = Application.persistentDataPath + "dataGame" + 6 + ".json";
        //StreamWriter streamReader = new StreamWriter(path);
        //Debug.Log(JsonUtility.ToJson(map));
        //streamReader.WriteLine(JsonUtility.ToJson(map));
        //streamReader.Close();
   
    }
    private void Update()
    {
        a += 99999;
        textt.text = a + "";
    }
    public List<MapData> GetDataAllMaps(int numberOfMap)
    {
        List<MapData> maps = new List<MapData>();
        for (int i = 0; i < numberOfMap; i++)
        {
            path = Application.persistentDataPath + "dataGame" + (i + 1) + ".json";
            if (!File.Exists(path))
            {
                StreamWriter streamWriter = new StreamWriter(path);
                streamWriter.Close();
            }
            StreamReader streamReader = new StreamReader(path);
            MapData map = JsonUtility.FromJson<MapData>(streamReader.ReadToEnd());
            streamReader.Close();
            maps.Add(map);
        }
        foreach (var map in maps)
        {
            Debug.Log(map);
        }
        return maps;
    }
}
[Serializable]
public class Data{
    public int a;
}
