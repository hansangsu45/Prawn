using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public enum Color_State
{
    BLACK,
    WHITE,
    RED,
    BLUE,
    YELLOW,
    GREEN,
    PURPLE,
    MAGENTA,
    CYAN,
    CLEAR,
    GRAY,
    ORANGE
}

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SaveData saveData;  
    public SaveData savedData { get { return saveData; } }
    public ShopManager shopManager;
    public Food food;
    public Dictionary<int, Prawn> idToPrawn;
    public Dictionary<Color_State, Color> eColor;
    private bool bQuitPanel = false;
    [SerializeField] GameObject[] g_objs;

    private Animator prawnAnimator;

    private string filePath;
    private readonly string SaveFileName = "Savefile";
    private string savedJson;
    private Coroutine AutoTchCo;  //자동으로 새우가 돈 벌게 하는 코루틴
    public GameObject BlackPanel;  //로딩 처리를 위한 검은색 바탕

    public MyPrawn myPrawn;
    public Sprite[] prawnSprs;
    public Text coinTxt;

    [Header("메인씬에서 쓰이는 껐다켰다하는 UI들")]
    public GameObject[] mainObjs;

    [Header("건들면 안됨")]
    public List<GameObject> uiObjs;   //뒤로가기 버튼으로 없앨 수 있는 UI들

    public Image hpImage, mentalImage;
    public Text hpTxt, mentalTxt, systemTxt;
    public GameObject[] menus;
    public int[] menusNum;

    public AudioSource _audio;
    public AudioClip[] _clips;

    #region 물고기 움직임 관련 변수
    //랜덤 위치 (maxPosition minPosition) 제한
    //랜덤 초 기다림 (max time min time) 제한
    private GameObject fishPooling;

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

    #region 피버 타임 관련 변수

    [Header("피버 타임 관련 변수")]
    [Tooltip("피버 타임 텍스트")]
    [SerializeField] private GameObject feverTimeText;
    [Tooltip("물고기 위치")]
    public Transform fishTransform;
    [Tooltip("배경 이미지 넣는곳")]
    [SerializeField] private SpriteRenderer background;
    [Tooltip("배경 이미지")]
    [SerializeField] private Sprite backgroundSprite;
    [Tooltip("피버 타임 배경 이미지")]
    [SerializeField] private Sprite fvTimeSprite;
    [Tooltip("피버 타임중 지워질 오브젝트")]
    [SerializeField] private GameObject removeObject;

    private float feverTime = 0f; //피버 타임
    private bool isFeverTime = false; //현재 피버타임인지 확인

    #endregion

    #region BGM 관련 변수

    [Header("BGM 관련 변수")]
    [Tooltip("배경 음악")]
    [SerializeField] private AudioClip backgroundBGM;
    [Tooltip("상점 배경 음악")]
    [SerializeField] private AudioClip shopBGM;
    [SerializeField] AudioClip feverBGM;
    [Tooltip("BGM을 재생하는곳")]
    [SerializeField] private AudioSource bgm;

    #endregion

    #region 이펙트 관련 변수

    [Header("이펙트 관련 변수")]
    [Tooltip("이펙트 1")]
    [SerializeField] private Animator effect1;
    [Tooltip("이펙트 2")]
    [SerializeField] private Animator effect2;

    private int touch = 0;

    #endregion

    private void Awake()
    {

#if UNITY_ANDROID
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1440, 2960, true);
#endif

        saveData = new SaveData();
        filePath = string.Concat(Application.persistentDataPath, "/", SaveFileName);

        Load();

        if (saveData.isFirstStart)
        {
            saveData.prawns.Add(new Prawn(10, 300,1 ,100, 100, 50, 0, 10, 10,2000, 300, "흰다리새우", "크기는 약20cm. 얕은 바다에 서식한다.전체적으로 대하와 유사하지만 더듬이와 수염이 짧다.", prawnSprs[0]));
            saveData.currentPrawn = saveData.prawns[0];
            DataLoad();
            saveData.isFirstStart = false;
        }

        prawnAnimator = myPrawn.GetComponent<Animator>();
        fishPooling = GameObject.Find("FishPooling");

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

        FeverTimeReset();

        bgm.clip = backgroundBGM;
        bgm.Play();
    }

#region 저장/로드
    public void Save()  //저장
    {
        SaveData();
        savedJson = JsonUtility.ToJson(saveData);
        byte[] bytes = Encoding.UTF8.GetBytes(savedJson);
        string code = Convert.ToBase64String(bytes);
        File.WriteAllText(filePath, code);
    }

    public void Load()  //불러오기
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
        InitDict();
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
        shopManager.foodCtnTxt.text = string.Format("보유중인 먹이 개수: {0}개", saveData.foodCount);
        shopManager.autoCoinLv.text = $"{saveData.autocoinLv}레벨";
        shopManager.autoBtnTxt.text = string.Format("자동 돈 복사 기계\n업그레이드({0}원)", saveData.autoCoinUpPrice);
        food.needFoodTxt.text = string.Format("먹이주기\n(필요 개수: {0}개)", saveData.currentPrawn.foodAmount);
    }
    private void InitDict()
    {
        idToPrawn = new Dictionary<int, Prawn>();

        for(int i=0; i<saveData.prawns.Count; i++)
            idToPrawn.Add(saveData.prawns[i].id, saveData.prawns[i]);

        eColor = new Dictionary<Color_State, Color>();

        eColor.Add(Color_State.BLACK, Color.black);
        eColor.Add(Color_State.BLUE, Color.blue);
        eColor.Add(Color_State.GREEN, Color.green);
        eColor.Add(Color_State.PURPLE, new Color(193,0,255,255));
        eColor.Add(Color_State.RED, Color.red);
        eColor.Add(Color_State.WHITE, Color.white);
        eColor.Add(Color_State.YELLOW, Color.yellow);
        eColor.Add(Color_State.MAGENTA, Color.magenta);
        eColor.Add(Color_State.CYAN, Color.cyan);
        eColor.Add(Color_State.CLEAR, Color.clear);
        eColor.Add(Color_State.GRAY, Color.gray);
        eColor.Add(Color_State.ORANGE, new Color(255, 99, 0, 255));
    }

#endregion

    public void Touch()  //화면 터치
    {
        if (isFeverTime)
        {
            _audio.clip = _clips[0];
            _audio.Play();
            saveData.coin += saveData.currentPrawn.power * 2;
            coinTxt.text = saveData.coin.ToString();
            saveData.currentPrawn.touchCount++;
        }
        else
        {
            if(saveData.currentPrawn.isRest)
            {
                ActiveSystemPanel("휴식이 필요합니다.");
                return;
            }

            if (saveData.currentPrawn.hp < saveData.currentPrawn.needHp || saveData.currentPrawn.curMental <= 0)
            {
                if(saveData.currentPrawn.curMental <= 0)
                {
                    saveData.currentPrawn.isRest = true;
                    saveData.currentPrawn.restStartTime = DateTime.Now.ToString();
                    SaveData();
                }
                ActiveSystemPanel("체력 혹은 정신력이 부족합니다.");
                return;
            }

            _audio.clip = _clips[0];
            _audio.Play();
            saveData.coin += saveData.currentPrawn.power;
            coinTxt.text = saveData.coin.ToString();
            saveData.currentPrawn.touchCount++;
            if (saveData.currentPrawn.touchCount % 5 == 0)
            {
                saveData.currentPrawn.curMental--;
                mentalImage.fillAmount = (float)saveData.currentPrawn.curMental / (float)saveData.currentPrawn.mental;
                mentalTxt.text = string.Format("MENTAL: {0}/{1}", saveData.currentPrawn.curMental, saveData.currentPrawn.mental);
            }

            saveData.currentPrawn.hp -= saveData.currentPrawn.needHp;
            hpImage.fillAmount = (float)saveData.currentPrawn.hp / (float)saveData.currentPrawn.maxHp;
            hpTxt.text = string.Format("HP: {0}/{1}", saveData.currentPrawn.hp, saveData.currentPrawn.maxHp);
        }

        prawnAnimator.Play("PrawnAnimation");
        effect1.Play("Effect");

        touch++;
        
        if (touch % 10 == 0)
        {
            effect2.Play("Effect2");
        }
        else
        {
            effect1.Play("Effect");
        }
    }

    public IEnumerator AutoTouch()  //자동 터치 코루틴
    {
        while(saveData.isAutoCoin)
        {
            yield return new WaitForSeconds(1);
            saveData.coin += saveData.autoCoin;
            coinTxt.text = saveData.coin.ToString();
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

                if (bQuitPanel) bQuitPanel = false;

                if (!mainObjs[0].activeSelf && !mainObjs[3].activeSelf&& !mainObjs[4].activeSelf)
                {
                    shopManager.MenuAni(false);
                    for (int i = 0; i < menus.Length; i++)
                    {
                        menus[i].transform.parent = mainObjs[0].transform;
                    }
                }

                if(uiObjs.Count==0&& bgm.clip != backgroundBGM)
                {
                    if (isFeverTime) bgm.clip = feverBGM;
                    else bgm.clip = backgroundBGM;
                    bgm.Play();
                }
            }
            else
            {
                bQuitPanel = true;
                ActiveSystemPanel("<b>게임을 종료하시겠습니까?</b>", Color_State.BLACK, 110);
            }
            _audio.clip = _clips[1];
            _audio.Play();
        }
        
        if (Input.GetMouseButtonDown(0)) ClickFish();

        FeverTimeCheck();
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

    public void GameQuit() => Application.Quit();

    public IEnumerator FadeEffect(Image img, float fadeTime=1f)  //이미지 fadein/out
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

    public void ButtonUIClick(int n)  //메인씬의 UI On/Off시 사용할 함수
    {
        if (mainObjs[n].activeSelf && bQuitPanel) bQuitPanel = false;

        if (mainObjs[n].activeSelf) uiObjs.Remove(mainObjs[n]);
        else uiObjs.Add(mainObjs[n]);

        mainObjs[n].SetActive(!mainObjs[n].activeSelf);
        shopManager.UpgradeRenewal();
        _audio.clip = _clips[1];
        _audio.Play();
        if (n == 0 && mainObjs[n].activeSelf)
        {
            bgm.clip = shopBGM;
            bgm.Play();
        }
        else if (uiObjs.Count == 0 && bgm.clip != backgroundBGM)
        {
            if (isFeverTime) bgm.clip = feverBGM;
            else bgm.clip = backgroundBGM;
            bgm.Play();
        }
    }

    public void ThreeMenu(int num)
    {
        if (!mainObjs[num].activeSelf)
        {
            shopManager.MenuAni(false);
            for (int i = 0; i < menus.Length; i++)
            {
                menus[i].transform.parent = mainObjs[0].transform;
            }
            return;
        }
        else
        {
            for (int i = 0; i < menus.Length; i++)
            {
                menus[i].transform.parent = mainObjs[num].transform;
            }
            for (int i = 0; i < menusNum.Length; i++)
            {
                if (menusNum[i] != num)
                {
                    if (mainObjs[menusNum[i]].activeSelf)
                    {
                        uiObjs.Remove(mainObjs[menusNum[i]]);
                        mainObjs[menusNum[i]].SetActive(false);
                    }
                }
            }
            shopManager.MenuAni(false);
        }
    }

    public void ActiveSystemPanel(string msg, Color_State _color=Color_State.BLACK, int font_size=95)  //시스템 메세지 띄우는 패널(함수)
    {
        mainObjs[1].SetActive(true);
        systemTxt.text = msg;
        systemTxt.color = eColor[_color];
        systemTxt.fontSize = font_size;
        uiObjs.Add(mainObjs[1]);

        g_objs[0].SetActive(!bQuitPanel);
        g_objs[1].SetActive(bQuitPanel);
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

            if (fishTransform == this.fishTransform)
            {
                randomPosition.y = 2.75f;
            }
            else
            {
                randomPosition.y = UnityEngine.Random.Range(fishMinPosition.y, fishMaxPosition.y);
            }
            
            fishTransform.position = randomPosition;
            fishTransform.SetParent(null, true);
            randomWaitSecond = UnityEngine.Random.Range(fishMinTime, fishMaxTime);
            yield return new WaitForSeconds(randomWaitSecond);
        }
    }

    public bool IsPrawnPossession(int compareId)  //어떤 새우가 현재 자신에게 보유중인지 체크할 함수
    {
        foreach(Prawn p in saveData.prawns)
        {
            if (p.id == compareId)
                return true;
        }
        return false; 
    }

    private void FeverTimeReset()
    {
        feverTime = Time.time;
        isFeverTime = false;
        feverTimeText.SetActive(false);
        background.sprite = backgroundSprite;
        removeObject.SetActive(true);
        if(uiObjs.Count==0||(uiObjs.Count==1&&mainObjs[1].activeSelf))
        {
            bgm.clip = backgroundBGM;
        }
        else
        {
            bgm.clip = shopBGM;
        }
        bgm.Play();
    }

    private void FeverTimeCheck()
    {
        if (isFeverTime)
        {
            if (Time.time >= feverTime + 10f)
            {
                FeverTimeReset();
            }
        }
    }

    public void ClickFish()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool isFishActive = fishTransform.gameObject.activeSelf;
        float distance = Vector3.Distance(fishTransform.position, mousePosition);

        if (distance < 10.01f && isFishActive)
        {
            isFeverTime = true;
            feverTimeText.SetActive(true);
            feverTime = Time.time;
            fishTransform.GetComponent<Fish>().FishDestroy();
            background.sprite = fvTimeSprite;
            removeObject.SetActive(false);
            bgm.clip = feverBGM;
            bgm.Play();
        } 
    }
}
