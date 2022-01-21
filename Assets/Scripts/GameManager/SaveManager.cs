using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Playables;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Planet planet;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private PlayableDirector introCutScene;

    private string savePath;

    private bool canSave;
    
    private void Start()
    {
        savePath = Application.persistentDataPath + "/save.qnd";
        StartCoroutine(WaitForLoadGame());
    }

    private IEnumerator WaitForLoadGame()
    {
        loadingPanel.SetActive(true);
        canSave = false;
        yield return new WaitUntil(LoadGame);
        canSave = true;
        loadingPanel.SetActive(false);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
            SaveGame();
    }

    private void OnApplicationQuit()
    {
        if(canSave)
            SaveGame();
    }

    private void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);
        Dictionary<int, Hexagon> hexagons = planet.SaveHexagons();
        SaveData data = new SaveData(gameManager, hexagons);
        bf.Serialize(stream, data);
        stream.Flush();
        stream.Close();
    }

    private bool LoadGame()
    {
        FileInfo file = new FileInfo(savePath);
        if (!File.Exists(savePath) || file.Length == 0)
        {
            planet.InitializeHexagons();
            introCutScene.Play();
            return true;
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Open);
        
        SaveData data = bf.Deserialize(stream) as SaveData;
        stream.Flush();
        stream.Close();
        // gameManager.stone = data.stoneAmount;
        planet.LoadHexagons(data.hexagons);
        return true;
    }

    public void ResetGameData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }
}

[Serializable]
class SaveData
{
    public int stoneAmount;
    public Dictionary<int, Hexagon> hexagons;

    public SaveData(GameManager gm, Dictionary<int, Hexagon> _hexagons)
    {
        // stoneAmount = gm.stone;
        hexagons = _hexagons;
    }
}
