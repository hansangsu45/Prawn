using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Button mainBtn;
    public Button SelectedPrawnButton;
    public ShopPrawnBtn SelectedPrawn;

    public void SelectPrawn(Button btn, ShopPrawnBtn spb)
    {
        SelectedPrawnButton = btn;
        SelectedPrawn = spb;
        mainBtn.image.sprite = spb.spr;
    }

    public void PrawnDetail()
    {
        //   패널 띄우기       보유중이면 설명,팔기,바꾸기 띄우기       그렇지않으면 구매하기 띄우기  
        foreach(Prawn p in GameManager.Instance.savedData.prawns)
        {
            if(p.id==SelectedPrawn.id)  //보유중인 새우면
            {

            }
            else  //보유중이지않은 새우면
            {

            }
        }
    }

    public void PurchasePrawn()
    {
        SelectedPrawn.ClickPurchase();
        //구매하기 버튼 없애고 설명,팔기,바꾸기 띄우기
    }

    public void SellPrawn()
    {
        foreach (Prawn p in GameManager.Instance.savedData.prawns)
        {
            if (p.id == SelectedPrawn.id)
            {
                GameManager.Instance.savedData.prawns.Remove(p);

                break;
            }
        }
        //구매하기 버튼 띄우기
    }

    public void ChangePrawn()
    {
        GameManager.Instance.SaveData();
        foreach (Prawn p in GameManager.Instance.savedData.prawns)
        {
            if (p.id == SelectedPrawn.id)
            {
                GameManager.Instance.savedData.currentPrawn = p;
                GameManager.Instance.myPrawn.PrawnLoad(p);
                GameManager.Instance.SetData();
                break;
            }
        }
    }
}
