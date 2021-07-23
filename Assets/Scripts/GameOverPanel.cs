using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text bestScoreText = null;

    private Training training = null;

    private void Awake()
    {
        training = FindObjectOfType<Training>();
    }

    private void OnEnable()
    {
        training.isGameOverPanelActive = true;
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        training.isGameOverPanelActive = false;
        Time.timeScale = 1f;
    }

    public void ShowScore(int curScore, int bestScore)
    {
        scoreText.text = "현재 점수: " + curScore;
        bestScoreText.text = "최고 점수: " + bestScore;
    }
}
