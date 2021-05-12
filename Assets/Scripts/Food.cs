using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    public void GiveFood()
    {
        int needFoodAm = GameManager.Instance.savedData.currentPrawn.foodAmount;

        if (GameManager.Instance.savedData.foodCount>=needFoodAm)
        {
            GameManager.Instance.savedData.foodCount -= needFoodAm;
            GameManager.Instance.savedData.currentPrawn.hp = GameManager.Instance.savedData.currentPrawn.maxHp;
        }
        else
        {
            GameManager.Instance.ActiveSystemPanel("먹이가 부족합니다!");
        }
    }
}
