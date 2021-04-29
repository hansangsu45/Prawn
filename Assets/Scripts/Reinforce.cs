using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforce : MonoBehaviour
{
    public void reinforce()
    {
        GameManager gameManager = GameManager.Instance;
        Prawn currentPrawn = gameManager.savedData.currentPrawn;
        long upgradePrice = currentPrawn.upgradePrice;

        if (gameManager.savedData.coin >= upgradePrice)
        {
            currentPrawn.level += 1;
            gameManager.savedData.coin -= upgradePrice;
            //여기부터 값이 달라짐
            currentPrawn.maxHp += 20;
            gameManager.savedData.currentPrawn = currentPrawn;
            gameManager.SetData();
        }
        else
        {
            gameManager.ActiveSystemPanel("돈이 부족합니다.");
        }
    }
}
