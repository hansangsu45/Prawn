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
        //   �г� ����       �������̸� ����,�ȱ�,�ٲٱ� ����       �׷��������� �����ϱ� ����  
        foreach(Prawn p in GameManager.Instance.savedData.prawns)
        {
            if(p.id==SelectedPrawn.id)  //�������� �����
            {

            }
            else  //�������������� �����
            {

            }
        }
    }

    public void PurchasePrawn()
    {
        SelectedPrawn.ClickPurchase();
        //�����ϱ� ��ư ���ְ� ����,�ȱ�,�ٲٱ� ����
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
        //�����ϱ� ��ư ����
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
