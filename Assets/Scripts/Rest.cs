using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Rest : MonoBehaviour
{
    private TimeSpan ts;
    private int[] ids;

    private void Start()
    {
        ids = new int[GameManager.Instance.shopManager.prawnBtns.Length];

        for(int i=0; i<ids.Length; i++)
        {
            ids[i] = GameManager.Instance.shopManager.prawnBtns[i].id;
        }

        StartCoroutine(PrawnRestCheck());
    }

    IEnumerator PrawnRestCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            for (int i = 0; i < ids.Length; i++)
            {
                if (GameManager.Instance.idToPrawn.ContainsKey(ids[i]))
                {
                    if (GameManager.Instance.idToPrawn[ids[i]].isRest)
                    {
                        ts = DateTime.Now - GameManager.Instance.idToPrawn[ids[i]].restStartTime;
                        //ts가 59를 넘으면 다시 1부터 시작하는 문제점 있다.
                        if(ts.Seconds>=GameManager.Instance.idToPrawn[ids[i]].restTime)
                        {
                            GameManager.Instance.idToPrawn[ids[i]].isRest = false;
                            GameManager.Instance.idToPrawn[ids[i]].curMental = GameManager.Instance.idToPrawn[ids[i]].mental;
                            GameManager.Instance.SetData();
                        }
                    }
                }
            }
        }
    }

}
