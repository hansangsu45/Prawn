using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPrawnBtn : MonoBehaviour  //상점에 있는 각각의 새우버튼들에 붙는 스크립트
{
    public int buyPrice;  //새우 가격

    [SerializeField] bool isAuto;
    public short id;
    [SerializeField] int maxHp;
    [SerializeField] int needHp;
    [SerializeField] int mental;
    [SerializeField] int str;
    [SerializeField] int def;
    [SerializeField] int sellPrice;
    [SerializeField] int foodAmt;
    [SerializeField] int maxTchCnt;
    [SerializeField] int power;
    [SerializeField] int auPower;
    [SerializeField] float restTime;
    [SerializeField] string _name;
    [SerializeField] string ex;
    [SerializeField] int up;
    public Sprite spr;
    public GameObject upgrade;

    public void ClickPurchase()  //새우 구매(ShopManager에서 사용될 함수)
    {
        if(GameManager.Instance.savedData.coin>=buyPrice)
        {
            GameManager.Instance.savedData.coin -= buyPrice;
            Prawn p = new Prawn(isAuto, id, maxHp, needHp, mental, str, def, sellPrice, foodAmt, maxTchCnt, power, auPower, up, restTime, _name, ex, spr);
            GameManager.Instance.savedData.prawns.Add(p);
            GameManager.Instance.idToPrawn.Add(id, p);
        }
        else
        {
            GameManager.Instance.ActiveSystemPanel("돈이 부족합니다.");
        }
    }

    public void ClickPrawnofShop() //해당 스크립트가 붙은 버튼 클릭 시
    {
        GameManager.Instance.shopManager.SelectPrawn(GetComponent<Button>(), this);
    }


    //bool isAuto, short id,int maxHp,int needHp ,int mental, int str, int def, int price, int foodAmount,int maxTouchCnt,int power,int auPower, float restTime, string name, Sprite spr
}
