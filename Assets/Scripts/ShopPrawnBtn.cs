using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPrawnBtn : MonoBehaviour  //������ �ִ� ������ �����ư�鿡 �ٴ� ��ũ��Ʈ
{
    public int buyPrice;  //���� ����

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

    public void ClickPurchase()  //���� ����(ShopManager���� ���� �Լ�)
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
            GameManager.Instance.ActiveSystemPanel("���� �����մϴ�.");
        }
    }

    public void ClickPrawnofShop() //�ش� ��ũ��Ʈ�� ���� ��ư Ŭ�� ��
    {
        GameManager.Instance.shopManager.SelectPrawn(GetComponent<Button>(), this);
    }


    //bool isAuto, short id,int maxHp,int needHp ,int mental, int str, int def, int price, int foodAmount,int maxTouchCnt,int power,int auPower, float restTime, string name, Sprite spr
}
