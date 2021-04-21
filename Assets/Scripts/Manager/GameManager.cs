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

    #region 물고기 움직임 관련 변수
    //랜덤 위치 (maxPosition minPosition) 제한
    //랜덤 초 기다림 (max time min time) 제한
    private FishPooling fishPooling;

    [Header("물고기 스폰 관련 변수")]
    [Tooltip("물고기 스폰 위치 최소 제한")]
    public Vector2 fishMinPosition;
    [Tooltip("물고기 스폰 위치 최대 제한")]
    public Vector2 fishMaxPosition;
    [Tooltip("물고기 스폰 최소 시간")]
    [SerializeField] private float fishMinTime;
    [Tooltip("물고기 스폰 최대 시간")]
    [SerializeField] private float fishMaxTime;

    #endregion

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
        fishPooling = FindObjectOfType<FishPooling>();

        #region (혹시 잘못되면 바로 알아볼 수 있도록 코드 추가함) fishPooling 관련 예외 처리 (유니티 안에서만 실행됨)
#if UNITY_EDITOR
        if (fishPooling == null) //fishPooling이 없다면 실행
        {
            UnityEditor.EditorUtility.DisplayDialog("FishPooling 오류", "FishPooling이 없습니다. FishPooling을 추가해 주세요", "확인");
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else if(fishPooling.transform.childCount <= 0)//fishPooling에 fish가 0개 이하라면 실행
        {
            UnityEditor.EditorUtility.DisplayDialog("FishPooling 오류", "FishPooling안에 Fish가 없습니다. Fish를 추가해 주세요", "확인");
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
        #endregion

        StartCoroutine(FadeEffect(BlackPanel.GetComponent<Image>()));
        StartCoroutine(SpawnFish());
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

    private IEnumerator SpawnFish()
    {
        yield return new WaitForSeconds(10f);

        Transform fishTransform = null;
        GameObject fishGO = null;

        Vector2 randomPosition = Vector2.zero;

        float randomWaitSecond = 0f;

        while (true)
        {
            if (fishPooling.transform.childCount > 0)
            {
                fishTransform = fishPooling.transform.GetChild(0);
                fishGO = fishTransform.gameObject;
            }
            else
            {
                yield return new WaitForSeconds(fishMinTime);
                continue;
            }

            fishGO.SetActive(true);
            randomPosition.x = fishMinPosition.x;
            randomPosition.y = UnityEngine.Random.Range(fishMinPosition.y, fishMaxPosition.y);
            fishTransform.position = randomPosition;
            fishTransform.SetParent(null, true);
            randomWaitSecond = UnityEngine.Random.Range(fishMinTime, fishMaxTime);
            yield return new WaitForSeconds(randomWaitSecond);
        }
    }
}
