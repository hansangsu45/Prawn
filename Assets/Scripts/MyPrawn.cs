using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPrawn : MonoBehaviour
{
    private Prawn prawn;

    [SerializeField] private Image prawnImage;
     
    public void PrawnLoad(Prawn p)
    {
        prawn = p;

        prawnImage.sprite = p.spr;
    }
}
