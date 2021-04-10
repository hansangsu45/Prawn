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
    public GameObject BlackPanel;

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
            saveData.prawns.Add(new Prawn(false, 10, 300, 100, 100, 50, 0, 10, 300, 1, 0, 300, "흰다리 새우", prawnSprs[0]));
            saveData.currentPrawn = saveData.prawns[0];
            myPrawn.PrawnLoad(saveData.currentPrawn);
            saveData.isFirstStart = false;
        }

        StartCoroutine(FadeEffect(BlackPanel.GetComponent<Image>()));
    }

    #region 저장/로드
    public void Save()
    {
        savedJson = JsonUtility.ToJson(saveData);
        byte[] bytes = Encoding.UTF8.GetBytes(savedJson);
        string code = Convert.ToBase64String(bytes);
        File.WriteAllText(filePath, code);
        SaveData();
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
        StartCoroutine(AutoTouch());
    }    
    private void SaveData()
    {
        foreach(Prawn p in saveData.prawns)
        {
            if(p.id==saveData.currentPrawn.id)
            {
                saveData.prawns[saveData.prawns.IndexOf(p)] = saveData.currentPrawn;
                return;
            }
        }
    }
    #endregion

    public void Touch()
    {
        saveData.coin += saveData.currentPrawn.power;
        coinTxt.text = saveData.coin.ToString();
        saveData.currentPrawn.touchCount++;
    }

    IEnumerator AutoTouch()
    {
        while(saveData.currentPrawn.isAutoWork)
        {
            saveData.coin += saveData.currentPrawn.autoPowor;
            coinTxt.text = saveData.coin.ToString();
            yield return new WaitForSeconds(1);
        }
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
        if (!focus)
        {
            Save();
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }

    public IEnumerator FadeEffect(Image img, float fadeTime=1f)
    {
        float t = fadeTime / 100;
        Color c = img.color;

        if(img.gameObject.activeSelf)
        {
            while(c.a>0)
            {
                c.a -= 0.01f;
                img.color = c;
                yield return new WaitForSeconds(t);
            }
            img.gameObject.SetActive(false);
        }
        else
        {
            img.gameObject.SetActive(true);
            c.a = 0;
            while(c.a!=1)
            {
                c.a += 0.01f;
                img.color = c;
                yield return new WaitForSeconds(t);
            }
        }
    }
}
