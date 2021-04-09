using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PrawnData
{
    public List<Prawn> prawns = new List<Prawn>();
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
    public string ex;
    public Sprite spr;

    public Prawn()
    {

    }
}
