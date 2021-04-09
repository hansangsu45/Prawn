using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public List<Prawn> prawns = new List<Prawn>();  //�������� ����
    public bool isFirstStart = true;  //���� ó�� ��������
    public long coin;  //�������� ��
    public Prawn currentPrawn;  //���� �������� ����
}

[Serializable]
public class Prawn
{
    public bool isAutoWork; //�ڵ����� �� ������

    public short id;

    public int maxHp;
    public int hp;
    public int mental;  //���ŷ�(����� ����)
    public int curMental; //���� ���ŷ�
    public int str;  //���ݷ�
    public int def;  //����
    public int price;
    public int foodAmount;  //�ʿ� ���̷�
    public int touchCount=0; //��ġ Ƚ��
    public int maxTouchCount;  //��ġ �ִ� Ƚ��(�ִ� Ƚ�� ���޽� �����ְų� �޽�)
    public int power; //1ȸ ��ġ�� ������̴� ��
    public int autoPowor; //�ʴ� �ڵ����� ������̴� ��

    public float restTime;  //�ʿ� �޽Ľð�
    public float currentRestTime;  //���� �޽��� �ð�

    public string name;

    public Sprite spr;

    public Prawn(bool isAuto, short id,int maxHp, int mental, int str, int def, int price, int foodAmount,int maxTouchCnt,int power,int auPower, float restTime, string name, Sprite spr)
    {
        this.isAutoWork = isAuto;
        this.id = id;
        this.maxHp = maxHp;
        this.mental = mental;
        this.str = str;
        this.def = def;
        this.price = price;
        this.foodAmount = foodAmount;
        this.maxTouchCount = maxTouchCnt;
        this.power = power;
        this.autoPowor = auPower;
        this.restTime = restTime;
        this.name = name;
        this.spr = spr;

        hp = maxHp;
        curMental = mental;
        currentRestTime = restTime;
    }
}
