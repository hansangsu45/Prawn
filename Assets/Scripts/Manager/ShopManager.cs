using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Button mainBtn;  //�������� �� ���� ū ��ư(��ư �̹����� �������� Ŭ���� ���� �̹�����)
    public Button SelectedPrawnButton; //�������� Ŭ���� ���� ��ư
    public ShopPrawnBtn SelectedPrawn;  //�������� ������ �����ư�� �پ��ִ� Ŭ����
    public ShopPrawnBtn[] prawnBtns;

    public GameObject possessionPanel;
    public GameObject noPossession;
    public Text prawnExplain;
    public Image prawnImage;

    public void SelectPrawn(Button btn, ShopPrawnBtn spb)  //�������� ���� ��ư Ŭ�� ��
    {
        SelectedPrawnButton = btn;
        SelectedPrawn = spb;
        mainBtn.image.sprite = spb.spr;
    }

    public void PrawnDetail()  //mainBtnŬ���ϸ� ���� ������ ���� �ڼ��� ���� �г��� Ȱ��ȭ �� ���� �Լ�
    {
        //   �г� ����       �������̸� ����,�ȱ�,�ٲٱ� ����       �׷��������� �����ϱ� ����
        bool isPoss = GameManager.Instance.IsPrawnPossession(SelectedPrawn.id);

        possessionPanel.SetActive(isPoss);
        noPossession.SetActive(!isPoss);
        if (isPoss)
        {
            prawnExplain.text = GameManager.Instance.savedData.currentPrawn.explain;
            prawnImage.sprite = GameManager.Instance.savedData.currentPrawn.spr;
        }
    }

    public void PurchasePrawn()  //���� ���� ��ư Ŭ��
    {
        SelectedPrawn.ClickPurchase();
        //�����ϱ� ��ư ���ְ� ����,�ȱ�,�ٲٱ� ����
    }

    public void SellPrawn()  //���� �ȱ�
    {
        if(GameManager.Instance.savedData.currentPrawn.id==SelectedPrawn.id)
        {
            GameManager.Instance.ActiveSystemPanel("�������� ����� �� �� �����ϴ�.", Color_State.RED,93);
            return;
        }
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

    public void UpgradeRenewal()
    {
        for(int i=0; i<prawnBtns.Length; i++)
        {
            if(GameManager.Instance.IsPrawnPossession(prawnBtns[i].id))
            {
                if(GameManager.Instance.savedData.coin>=GameManager.Instance.idToPrawn[prawnBtns[i].id].upgradePrice)
                {
                    prawnBtns[i].upgrade.SetActive(true);
                }
                else
                {
                    prawnBtns[i].upgrade.SetActive(false);
                }
            }
        }
    }
}
