using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Button mainBtn;  //상점에서 맨 위의 큰 버튼(버튼 이미지는 상점에서 클릭한 새우 이미지로)
    public Button SelectedPrawnButton; //상점에서 클릭한 새우 버튼
    public ShopPrawnBtn SelectedPrawn;  //상점에서 각각의 새우버튼에 붙어있는 클래스
    public ShopPrawnBtn[] prawnBtns;

    public GameObject possessionPanel;
    public GameObject noPossession;
    public Text prawnExplain;
    public Image prawnImage;

    public void SelectPrawn(Button btn, ShopPrawnBtn spb)  //상점에서 새우 버튼 클릭 시
    {
        SelectedPrawnButton = btn;
        SelectedPrawn = spb;
        mainBtn.image.sprite = spb.spr;
    }

    public void PrawnDetail()  //mainBtn클릭하면 새로 나오는 새우 자세히 보기 패널을 활성화 할 때의 함수
    {
        //   패널 띄우기       보유중이면 설명,팔기,바꾸기 띄우기       그렇지않으면 구매하기 띄우기
        bool isPoss = GameManager.Instance.IsPrawnPossession(SelectedPrawn.id);

        possessionPanel.SetActive(isPoss);
        noPossession.SetActive(!isPoss);
        if (isPoss)
        {
            prawnExplain.text = GameManager.Instance.savedData.currentPrawn.explain;
            prawnImage.sprite = GameManager.Instance.savedData.currentPrawn.spr;
        }
    }

    public void PurchasePrawn()  //새우 구입 버튼 클릭
    {
        SelectedPrawn.ClickPurchase();
        //구매하기 버튼 없애고 설명,팔기,바꾸기 띄우기
    }

    public void SellPrawn()  //새우 팔기
    {
        if(GameManager.Instance.savedData.currentPrawn.id==SelectedPrawn.id)
        {
            GameManager.Instance.ActiveSystemPanel("착용중인 새우는 팔 수 없습니다.", Color_State.RED,93);
            return;
        }
        GameManager.Instance.savedData.coin += GameManager.Instance.idToPrawn[SelectedPrawn.id].price;
        GameManager.Instance.SetData();
        GameManager.Instance.savedData.prawns.Remove(GameManager.Instance.idToPrawn[SelectedPrawn.id]);
        GameManager.Instance.idToPrawn.Remove(SelectedPrawn.id);
        //구매하기 버튼 띄우기
    }

    public void ChangePrawn()  //새우 교체
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
