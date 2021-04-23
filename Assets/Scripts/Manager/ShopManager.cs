using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Button mainBtn;  //�������� �� ���� ū ��ư(��ư �̹����� �������� Ŭ���� ���� �̹�����)
    public Button SelectedPrawnButton; //�������� Ŭ���� ���� ��ư
    public ShopPrawnBtn SelectedPrawn;  //�������� ������ �����ư�� �پ��ִ� Ŭ����

    public void SelectPrawn(Button btn, ShopPrawnBtn spb)  //�������� ���� ��ư Ŭ�� ��
    {
        SelectedPrawnButton = btn;
        SelectedPrawn = spb;
        mainBtn.image.sprite = spb.spr;
    }

    public void PrawnDetail()  //mainBtnŬ���ϸ� ���� ������ ���� �ڼ��� ���� �г��� Ȱ��ȭ �� ���� �Լ�
    {
        //   �г� ����       �������̸� ����,�ȱ�,�ٲٱ� ����       �׷��������� �����ϱ� ����
        if (GameManager.Instance.IsPrawnPossession(SelectedPrawn.id))  //�������� �����
        {

        }
        else   //�������������� �����
        {

        }
    }

    public void PurchasePrawn()  //���� ���� ��ư Ŭ��
    {
        SelectedPrawn.ClickPurchase();
        //�����ϱ� ��ư ���ְ� ����,�ȱ�,�ٲٱ� ����
    }

    public void SellPrawn()  //���� �ȱ�
    {
        GameManager.Instance.savedData.coin += GameManager.Instance.idToPrawn[SelectedPrawn.id].price;
        GameManager.Instance.SetData();
        GameManager.Instance.savedData.prawns.Remove(GameManager.Instance.idToPrawn[SelectedPrawn.id]);
        GameManager.Instance.idToPrawn.Remove(SelectedPrawn.id);
        //�����ϱ� ��ư ����
    }

    public void ChangePrawn()  //���� ��ü
    {
        GameManager.Instance.SaveData();

        Prawn p= GameManager.Instance.idToPrawn[SelectedPrawn.id];

        GameManager.Instance.savedData.currentPrawn = p;
        GameManager.Instance.myPrawn.PrawnLoad(p);
        GameManager.Instance.SetData();
    }
}
