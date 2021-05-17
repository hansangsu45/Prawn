using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    public InputField foodInput;
    public Text needFoodTxt;
    [SerializeField] int foodPrice = 50;

    public void GiveFood()
    {
        int needFoodAm = GameManager.Instance.savedData.currentPrawn.foodAmount;

        if (GameManager.Instance.savedData.foodCount>=needFoodAm)
        {
            GameManager.Instance.savedData.foodCount -= needFoodAm;
            GameManager.Instance.savedData.currentPrawn.hp = GameManager.Instance.savedData.currentPrawn.maxHp;
            GameManager.Instance.SetData();
        }
        else
        {
            GameManager.Instance.ActiveSystemPanel("먹이가 부족합니다!");
        }
    }

    public void ChargeFood()
    {
        if (foodInput.text.Trim() == "")
        {
            GameManager.Instance.ActiveSystemPanel("구입할 개수를 입력하세요");
            return;
        }

        int price = int.Parse(foodInput.text) * foodPrice;

        if (GameManager.Instance.savedData.coin>=price)
        {
            GameManager.Instance.savedData.foodCount += int.Parse(foodInput.text);
            GameManager.Instance.savedData.coin -= price;
            GameManager.Instance.SetData();
            foodInput.text = "";
            GameManager.Instance.ButtonUIClick(5);
        }
        else
        {
            GameManager.Instance.ActiveSystemPanel("돈이 부족합니다.");
        }
    }

    
}
