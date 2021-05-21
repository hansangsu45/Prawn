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
                        ts = DateTime.Now - Convert.ToDateTime(GameManager.Instance.idToPrawn[ids[i]].restStartTime);
                        //Debug.Log(ts.TotalSeconds);
                        if (ts.TotalSeconds>=GameManager.Instance.idToPrawn[ids[i]].restTime)
                        {
                            GameManager.Instance.idToPrawn[ids[i]].isRest = false;
                            GameManager.Instance.idToPrawn[ids[i]].curMental = GameManager.Instance.idToPrawn[ids[i]].mental;

                            if(ids[i]==GameManager.Instance.savedData.currentPrawn.id)
                            {
                                GameManager.Instance.savedData.currentPrawn.isRest = false;
                                GameManager.Instance.savedData.currentPrawn.curMental = GameManager.Instance.savedData.currentPrawn.mental;
                            }

                            GameManager.Instance.SetData();
                        }
                    }
                }
            }
        }
    }
}
