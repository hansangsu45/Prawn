using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforce : MonoBehaviour
{
    [SerializeField] private int minFoodAmount = 3;
    [SerializeField] private int minRestTime = 60;

    public void reinforce()
    {
        GameManager gameManager = GameManager.Instance;
        Prawn currentPrawn = gameManager.savedData.currentPrawn;
        long upgradePrice = currentPrawn.upgradePrice;

        if (gameManager.savedData.coin >= upgradePrice)
        {
            currentPrawn.level += 1;
            gameManager.savedData.coin -= upgradePrice;

            currentPrawn.maxHp += 50;
            currentPrawn.hp = currentPrawn.maxHp;
            currentPrawn.str += 110;
            currentPrawn.def += 5;

            if (currentPrawn.restTime > minRestTime)
            {
                currentPrawn.restTime -= 30;

                if(currentPrawn.restTime < minRestTime)
                {
                    currentPrawn.restTime = minRestTime;
                }
            }

            if (currentPrawn.foodAmount > minFoodAmount)
            {
                currentPrawn.foodAmount -= 1;

                if (currentPrawn.foodAmount < minFoodAmount)
                {
                    currentPrawn.foodAmount = minFoodAmount;
                }
            }

            gameManager.savedData.currentPrawn = currentPrawn;
            gameManager.SetData();
        }
        else
        {
            gameManager.ActiveSystemPanel("돈이 부족합니다.");
        }
    }
}
