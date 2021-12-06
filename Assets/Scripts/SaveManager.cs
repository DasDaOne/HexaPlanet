using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Planet planet;
    private string savePath;
    
    private void Start()
    {
        LoadGame();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
            SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    void SaveGame()
    {
        savePath = Application.persistentDataPath + "/save.qnd";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);
        Dictionary<int, Hexagon> hexagons = planet.SaveHexagons();
        SaveData data = new SaveData(gameManager, hexagons);
        bf.Serialize(stream, data);
        stream.Flush();
        stream.Close();
    }

    void LoadGame()
    {
        savePath = Application.persistentDataPath + "/save.qnd";
        if (!File.Exists(savePath))
        {
            planet.InitializeHexagons();
            return;
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Open);
        SaveData data = bf.Deserialize(stream) as SaveData;
        stream.Flush();
        stream.Close();
        gameManager.stone = data.stoneAmount;
        if(data.hexagons != null)
            planet.LoadHexagons(data.hexagons);
        else
            planet.InitializeHexagons();
    }
}

[Serializable]
class SaveData
{
    public int stoneAmount;
    public Dictionary<int, Hexagon> hexagons;

    public SaveData(GameManager gm, Dictionary<int, Hexagon> _hexagons)
    {
        stoneAmount = gm.stone;
        hexagons = _hexagons;
    }
}
