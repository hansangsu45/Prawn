using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private Training training = null;

    private void Awake()
    {
        training = FindObjectOfType<Training>();
    }

    private void OnEnable()
    {
        if (training.isGameOverPanelActive)
        {
            GameManager.Instance.ButtonUIClick(8);
        }

        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
