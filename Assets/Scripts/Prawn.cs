using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public List<Prawn> prawns = new List<Prawn>();  //보유중인 새우
    public bool isFirstStart = true;  //게임 처음 시작인지
    public long coin;  //보유중인 돈
    public int foodCount;  //먹이 수
    public bool isAutoCoin = false;  //자동으로 돈 버는지
    public int autoCoin = 0;  //자동으로 버는 돈
    public int nextAutoCoin = 20; //다음 레벨 자동으로 버는 돈
    public int autoCoinUpPrice = 40; //자동 코인 업글 비용
    public int autocoinLv = 0;
    public Prawn currentPrawn;  //현재 착용중인 새우
}

[Serializable]
public class Prawn
{
    public bool isAutoWork; //자동으로 돈 버는지

    public short id;

    public int level = 1;
    public int maxHp;
    public int hp;
    public int needHp;  //터치당 필요 닳는 체력
    public int mental;  //정신력(배고픔 상태)
    public int curMental; //현재 정신력
    public int str;  //공격력
    public int def;  //방어력
    public int price;  // 판매 가격
    public int foodAmount;  //필요 먹이량
    public int touchCount=0; //터치 횟수
    //public int maxTouchCount;  //터치 최대 횟수(최대 횟수 도달시 먹이주거나 휴식)
    public int power; //1회 터치당 벌어들이는 돈
    public int autoPowor; //초당 자동으로 벌어들이는 돈
    public int upgradePrice; //업그레이드에 필요한 돈

    public float restTime;  //필요 휴식시간
    public float currentRestTime;  //현재 휴식한 시간

    public string name;
    public string explain;

    public Sprite spr;

    public Prawn(bool isAuto, short id,int maxHp,int needHp ,int mental, int str, int def, int price, int foodAmount,int power,int auPower,int up , float restTime, string name,string ex ,Sprite spr)
    {
        this.isAutoWork = isAuto;
        this.id = id;
        this.maxHp = maxHp;
        this.needHp = needHp;
        this.mental = mental;
        this.str = str;
        this.def = def;
        this.price = price;
        this.foodAmount = foodAmount;
        //this.maxTouchCount = maxTouchCnt;
        this.power = power;
        this.autoPowor = auPower;
        this.upgradePrice = up;
        this.restTime = restTime;
        this.name = name;
        this.explain = ex;
        this.spr = spr;

        hp = maxHp;
        curMental = mental;
        currentRestTime = restTime;
    }
}
