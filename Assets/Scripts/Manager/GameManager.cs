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
    public SaveData savedData { get { return saveData; } }
    public ShopManager shopManager;

    private Animator prawnAnimator;

    private string filePath;
    private readonly string SaveFileName = "Savefile";
    string savedJson;
    Coroutine AutoTchCo;
    public GameObject BlackPanel;

    public MyPrawn myPrawn;
    public Sprite[] prawnSprs;
    public Text coinTxt;

    public GameObject[] mainObjs;
    public List<GameObject> uiObjs;

    public Image hpImage, mentalImage;
    public Text hpTxt, mentalTxt, systemTxt;

    private void Awake()
    {
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1440, 2960, true);

        saveData = new SaveData();
        filePath = string.Concat(Application.persistentDataPath, "/", SaveFileName);

        Load();

        if (saveData.isFirstStart)
        {
            saveData.prawns.Add(new Prawn(false, 10, 300,1 ,100, 100, 50, 0, 10, 300, 10, 0, 300, "흰다리 새우","(기본 새우 설명)" ,prawnSprs[0]));
            saveData.currentPrawn = saveData.prawns[0];
            DataLoad();
            saveData.isFirstStart = false;
        }

        prawnAnimator = myPrawn.GetComponent<Animator>();

        StartCoroutine(FadeEffect(BlackPanel.GetComponent<Image>()));
    }

    #region 저장/로드
    public void Save()
    {
        SaveData();
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
        SetData();
        AutoTchCo =StartCoroutine(AutoTouch());
    }    
    public void SaveData()
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
    public void SetData()
    {
        coinTxt.text = saveData.coin.ToString();
        hpImage.fillAmount = (float)saveData.currentPrawn.hp / (float)saveData.currentPrawn.maxHp;
        hpTxt.text = string.Format("HP: {0}/{1}", saveData.currentPrawn.hp, saveData.currentPrawn.maxHp);
        mentalImage.fillAmount = (float)saveData.currentPrawn.curMental / (float)saveData.currentPrawn.mental;
        mentalTxt.text = string.Format("MENTAL: {0}/{1}", saveData.currentPrawn.curMental, saveData.currentPrawn.mental);
    }

    #endregion

    public void Touch()
    {
        if (saveData.currentPrawn.hp < saveData.currentPrawn.needHp || saveData.currentPrawn.curMental <= 0)
            return;

        saveData.coin += saveData.currentPrawn.power;
        coinTxt.text = saveData.coin.ToString();
        saveData.currentPrawn.touchCount++;
        if(saveData.currentPrawn.touchCount%5==0)
        {
            saveData.currentPrawn.curMental--;
            mentalImage.fillAmount = (float)saveData.currentPrawn.curMental / (float)saveData.currentPrawn.mental;
            mentalTxt.text = string.Format("MENTAL: {0}/{1}", saveData.currentPrawn.curMental, saveData.currentPrawn.mental);
        }

        saveData.currentPrawn.hp -= saveData.currentPrawn.needHp;
        hpImage.fillAmount = (float)saveData.currentPrawn.hp / (float)saveData.currentPrawn.maxHp;
        hpTxt.text = string.Format("HP: {0}/{1}", saveData.currentPrawn.hp, saveData.currentPrawn.maxHp);
        prawnAnimator.Play("PrawnAnimation");
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(uiObjs.Count>0)
            {
                uiObjs[uiObjs.Count - 1].SetActive(false);
                uiObjs.RemoveAt(uiObjs.Count - 1);
            }
            else
            {
                //게임종료 패널 띄우기
            }
        }
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

    public void ButtonUIClick(int n)
    {
        if (mainObjs[n].activeSelf) uiObjs.Remove(mainObjs[n]);
        else uiObjs.Add(mainObjs[n]);

        mainObjs[n].SetActive(!mainObjs[n].activeSelf);
    }

    public void ActiveSystemPanel(string msg)
    {
        mainObjs[1].SetActive(true);
        systemTxt.text = msg;
        uiObjs.Add(mainObjs[1]);
    }
}
