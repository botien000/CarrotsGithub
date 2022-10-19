using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    private static string path;
    /// <summary>
    /// Singleton
    /// </summary>
    public static DataManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        //PlayerPrefs.SetInt("FirstTimeToUse", 0);
        int first = PlayerPrefs.GetInt("FirstTimeToUse", 0);
        if (first == 0)
        {
            PlayerPrefs.SetInt("FirstTimeToUse", 1);
            SaveDataMap(1, new MapData(1));
            //SaveDataMap(2, new MapData(2));
            //SaveDataMap(3, new MapData(3));
            //SaveDataMap(4, new MapData(4));
            //SaveDataMap(5, new MapData(5));
        }
    }

    public MapData GetDataMap(int typeMap)
    {
        path = Application.persistentDataPath + "dataGame" + typeMap + ".json";
        StreamReader streamReader = new StreamReader(path);
        MapData map = JsonUtility.FromJson<MapData>(streamReader.ReadToEnd());
        streamReader.Close();
        return map;
    }
    ///<sumary>
    ///Lưu star,score,map vào PlayerPref
    ///</sumary>
    ///<param name="level">Level</param>
    ///<param name="star">Star</param>
    ///<param name="score">Score</param>
    public void SaveDataMap(int typeMap, MapData map)
    {
        path = Application.persistentDataPath + "dataGame" + typeMap + ".json";
        StreamWriter streamReader = new StreamWriter(path);
        streamReader.WriteLine(JsonUtility.ToJson(map));
        streamReader.Close();
    }

    /// <summary>
    /// Get toàn bộ dữ liệu của 5 map
    /// </summary>
    /// <returns>List Map</returns>
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
        return maps;
    }
    /// <summary>
    /// Turn on or off sound
    /// </summary>
    /// <param name="type"></param>
    public void SetSound(int type)
    {
        PlayerPrefs.SetInt("Sound", type);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// GetSound
    /// </summary>
    /// <returns>Int</returns>
    public int GetSound()
    {
        return PlayerPrefs.GetInt("Sound", 1);
    }
    /// <summary>
    /// Turn on or off music
    /// </summary>
    /// <param name="type"></param>
    public void SetMusic(int type)
    {
        PlayerPrefs.SetInt("Music", type);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// GetMusic
    /// </summary>
    /// <returns>Int</returns>
    public int GetMusic()
    {
        return PlayerPrefs.GetInt("Music", 1);
    }
}
