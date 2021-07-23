using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Training : MonoBehaviour
{
    public Text bestScoreTxt;
    public GameObject TrainingGamePanel;
    public Canvas mainCvs, trainingCvs;


    private Queue<GameObject> queue = new Queue<GameObject>();
    public GameObject enemyPrefab;
    private Queue<GameObject> queue2 = new Queue<GameObject>();
    public GameObject foodPrefab;

    [HideInInspector] public bool isTraining = false;  //�Ʒ� ���ӿ� �����ߴ���
    public TrainingPrawn prawn;
    public Text curScoreTxt; //���� ���� �ؽ�Ʈ

    [SerializeField] private int maxLife=3;  //�ִ� ���
    [SerializeField] private int life;  // ���� ���
    [SerializeField] private float maxRange; 
    [SerializeField] private float time;  //�帥 �ð�

    [SerializeField] private float xMin, xMax;  //�� ��ȯ ������ X��
    [SerializeField] private float enemyY;  //�� ��ȯ ��ġ Y��

    [SerializeField] private float delayMax;
    private float dTime; 
    private int score;
    private bool isGameEnd;  //�׾��°�? (�Ʒ� ���ӿ��� ������ ����)

    public GameOverPanel gameOverPanel;
    [HideInInspector] public bool isGameOverPanelActive = false;

    private IEnumerator enemyCoroutine = null;
    private IEnumerator foodCoroutine = null;

    [Header("ü�� ǥ��")]
    [SerializeField] private Image[] lifeImages;
    [SerializeField] private Color damageDisplayColor;

    private void Start() //���׸� ������Ʈ Ǯ�� ���°� ������ �ϴ� ���ϴ� �̷��� ��
    {
        bestScoreTxt.text = string.Format("�ְ�����: {0}��", GameManager.Instance.savedData.trainingBestScore);

        for(int i=0; i<30; i++)
        {
            GameObject o = Instantiate(enemyPrefab, Vector2.zero, Quaternion.identity,prawn.transform.parent);
            InsertQueue(o,true);

            GameObject g = Instantiate(foodPrefab, Vector2.zero, Quaternion.identity, prawn.transform.parent);
            InsertQueue(g,false);
        }
    }

    private void Update()
    {
        if (isTraining && !isGameEnd)
        {
            time += Time.deltaTime;

            if ((int)time % 20 == 0 && delayMax > 0.5f && dTime<Time.time)
            {
                delayMax -= 0.2f;
                dTime = Time.time + 3f;
            }
        }
    }

    public void StartOrQuit(bool isStart)  //�Ʒ� �����̳� ����
    {
        trainingCvs.gameObject.SetActive(isStart);
        mainCvs.gameObject.SetActive(!isStart);

        isTraining = isStart;
        TrainingGamePanel.SetActive(isStart);

        GameManager.Instance.isTraining = isStart;

        if(isStart)
        {
            foreach (Image lifeImage in lifeImages)
            {
                lifeImage.sprite = GameManager.Instance.savedData.currentPrawn.spr;
            }

            prawn.spr.sprite = GameManager.Instance.savedData.currentPrawn.spr;
            GameReset();
        }
    }

    public void GameReset() //���� �ٽý���
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy.gameObject.activeSelf)
            {
                InsertQueue(enemy.gameObject, enemy.isEnemy);
            }
        }

        if (enemyCoroutine != null) StopCoroutine(enemyCoroutine);
        if (foodCoroutine != null) StopCoroutine(foodCoroutine);

        score = 0;
        curScoreTxt.text = string.Format("Score: {0}", score);
        isGameEnd = false;
        life = maxLife;
        delayMax = maxRange;
        time = 0;

        foreach (Image lifeImage in lifeImages)
        {
            lifeImage.color = Color.white;
        }

        enemyCoroutine = SpawnEnemy();
        foodCoroutine = SpawnFood();

        StartCoroutine(enemyCoroutine);
        StartCoroutine(foodCoroutine);
    }

    public void HitPlayer()  //�ĸ���
    {
        if (life <= 0) return;

        life--;

        lifeImages[life].color = damageDisplayColor;

        if (life == 0) GameOver();
    }

    public void AteFood()  //���� ����
    {
        if (isGameEnd) return;

        score++;
        curScoreTxt.text = string.Format("Score: {0}", score);
    }

    public void GameOver()  //�׾��� ��
    {
        if (score > GameManager.Instance.savedData.trainingBestScore)
        {
            GameManager.Instance.savedData.trainingBestScore = score;
            bestScoreTxt.text = string.Format("�ְ�����: {0}��", score);
        }

        isGameEnd = true;

        GameManager.Instance.ButtonUIClick(9);
        gameOverPanel.ShowScore(score, GameManager.Instance.savedData.trainingBestScore);
    }


    public void InsertQueue(GameObject o, bool isEnemy)
    {
        o.transform.position = Vector2.zero;

        if (isEnemy)
            queue.Enqueue(o);
        else
            queue2.Enqueue(o);

        o.SetActive(false);
    }

    public GameObject GetQueue(bool isEnemy)
    {
        GameObject o;

        if (isEnemy)
        {
            if (queue.Count > 0)
            {
                o = queue.Dequeue();
                o.SetActive(true);
            }
            else
            {
                o = Instantiate(enemyPrefab, Vector2.zero, Quaternion.identity, prawn.transform.parent);
            }
        }
        else
        {
            if (queue2.Count > 0)
            {
                o = queue2.Dequeue();
                o.SetActive(true);
            }
            else
            {
                o = Instantiate(foodPrefab, Vector2.zero, Quaternion.identity, prawn.transform.parent);
            }
        }

        return o;
    }    

    IEnumerator SpawnEnemy()  //�� ��ȯ
    {
        while(isTraining && !isGameEnd)
        {
            float delay = Random.Range(0.3f, delayMax);
            yield return new WaitForSeconds(delay);

            GameObject e = GetQueue(true);
            e.transform.position = new Vector2(Random.Range(xMin, xMax), enemyY);
        }

        enemyCoroutine = null;
    }

    IEnumerator SpawnFood()
    {
        while (isTraining && !isGameEnd)
        {
            yield return new WaitForSeconds(Random.Range(0.6f,3.5f));

            GameObject f = GetQueue(false);
            f.transform.position = new Vector2(Random.Range(xMin, xMax), enemyY);
        }

        foodCoroutine = null;
    }

    public void ReStart(bool gameOver)
    {
        if (gameOver)
        {
            GameManager.Instance.ButtonUIClick(9);
            GameReset();
        }
        else
        {
            GameManager.Instance.ButtonUIClick(8);
            GameReset();
        }
    }

    public void Continue()
    {
        GameManager.Instance.ButtonUIClick(8);
    }

    public void Exit(bool gameOver)
    {
        if (gameOver)
        {
            GameManager.Instance.ButtonUIClick(9);
            StartOrQuit(false);
        }
        else
        {
            GameManager.Instance.ButtonUIClick(8);
            StartOrQuit(false);
        }
    }
}
