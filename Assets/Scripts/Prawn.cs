using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public List<Prawn> prawns = new List<Prawn>();  //�������� ����

    public bool isFirstStart = true;  //���� ó�� ��������

    public long coin;  //�������� ��
    public int foodCount;  //���� ��

    public bool isAutoCoin = false;  //�ڵ����� �� ������
    public int autoCoin = 0;  //�ڵ����� ���� ��
    public int nextAutoCoin = 20; //���� ���� �ڵ����� ���� ��
    public int autoCoinUpPrice = 40; //�ڵ� ���� ���� ���
    public int autocoinLv = 0;

    public Prawn currentPrawn;  //���� �������� ����
}

[Serializable]
public class Prawn
{
    public short id;

    public bool isRest;

    public int level = 1;
    public int maxHp;
    public int hp;
    public int needHp;  //��ġ�� �ʿ� ��� ü��
    public int mental;  //���ŷ�(����� ����)
    public int curMental; //���� ���ŷ�
    public int str;  //���ݷ�
    public int def;  //����
    public int price;  // �Ǹ� ����
    public int foodAmount;  //�ʿ� ���̷�
    public int touchCount=0; //��ġ Ƚ��
    public int power; //1ȸ ��ġ�� ������̴� ��
    public int upgradePrice; //���׷��̵忡 �ʿ��� ��

    public float restTime;  //�ʿ� �޽Ľð� (�� ����)

    public string name;
    public string explain;

    public Sprite spr;

    public DateTime restStartTime;

    public Prawn(short id,int maxHp,int needHp ,int mental, int str, int def, int price, int foodAmount,int power,int up , float restTime, string name,string ex ,Sprite spr)
    {
        this.id = id;
        this.maxHp = maxHp;
        this.needHp = needHp;
        this.mental = mental;
        this.str = str;
        this.def = def;
        this.price = price;
        this.foodAmount = foodAmount;
        this.power = power;
        this.upgradePrice = up;
        this.restTime = restTime;
        this.name = name;
        this.explain = ex;
        this.spr = spr;

        hp = maxHp;
        curMental = mental;
        isRest = false;
    }
}
