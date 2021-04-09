using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] SaveData saveData;
    public SaveData savedData { get; }

    private string filePath;
    private readonly string SaveFileName = "Savefile";
    string savedJson;

    [SerializeField] private MyPrawn myPrawn;
    public Sprite[] prawnSprs;
    public Text coinTxt;

    private void Awake()
    {
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        saveData = new SaveData();
        filePath = string.Concat(Application.persistentDataPath, "/", SaveFileName);
        Load();

        if (saveData.isFirstStart)
        {
            saveData.prawns.Add(new Prawn(false, 10, 300, 100, 100, 50, 0, 10, 300, 1, 0, 300, "Èò´Ù¸® »õ¿ì", prawnSprs[0]));
            saveData.currentPrawn = saveData.prawns[0];
            myPrawn.PrawnLoad(saveData.currentPrawn);
            saveData.isFirstStart = false;
        }
    }

    public void Save()
    {
        savedJson = JsonUtility.ToJson(saveData);
        byte[] bytes = Encoding.UTF8.GetBytes(savedJson);
        string code = Convert.ToBase64String(bytes);
        File.WriteAllText(filePath, code);
    }

    public void Load()
    {
        if(File.Exists(filePath))
        {
            string code = File.ReadAllText(filePath);
            byte[] bytes = Convert.FromBase64String(code);
            savedJson = Encoding.UTF8.GetString(bytes);
            saveData = JsonUtility.FromJson<SaveData>(savedJson);
            DataLoad();
        }
    }
    private void DataLoad()
    { 
        myPrawn.PrawnLoad(saveData.currentPrawn);
        coinTxt.text = saveData.coin.ToString();
    }    
    public void Touch()
    {
        saveData.coin += saveData.currentPrawn.power;
        coinTxt.text = saveData.coin.ToString();
    }

    private void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationFocus(bool focus)
    {
        Save();
    }
    private void OnApplicationPause(bool pause)
    {
        Save();
    }
}
