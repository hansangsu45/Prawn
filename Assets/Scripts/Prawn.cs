using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public List<Prawn> prawns = new List<Prawn>();
    public bool isFirstStart = false;
}

[Serializable]
public class Prawn
{
    public short id;
    public int hp;
    public int mental;
    public int str;
    public int def;
    public int price;
    public int foodAmount;
    public float restTime;
    public float currentRestTime;

    public string name;
    public string ex;
    public Sprite spr;

    public Prawn(short id, int hp, int mental, int str, int def, int price, int foodAmount, float restTime,float currentRestTime,string name,string ex, Sprite spr)
    {
        this.id = id;
        this.hp = hp;
        this.mental = mental;
        this.str = str;
        this.def = def;
        this.price = price;
        this.foodAmount = foodAmount;
        this.restTime = restTime;
        this.currentRestTime = currentRestTime;
        this.name = name;
        this.ex = ex;
        this.spr = spr;
    }
}
