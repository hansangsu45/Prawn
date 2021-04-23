using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Button mainBtn;  //상점에서 맨 위의 큰 버튼(버튼 이미지는 상점에서 클릭한 새우 이미지로)
    public Button SelectedPrawnButton; //상점에서 클릭한 새우 버튼
    public ShopPrawnBtn SelectedPrawn;  //상점에서 각각의 새우버튼에 붙어있는 클래스

    public void SelectPrawn(Button btn, ShopPrawnBtn spb)  //상점에서 새우 버튼 클릭 시
    {
        SelectedPrawnButton = btn;
        SelectedPrawn = spb;
        mainBtn.image.sprite = spb.spr;
    }

    public void PrawnDetail()  //mainBtn클릭하면 새로 나오는 새우 자세히 보기 패널을 활성화 할 때의 함수
    {
        //   패널 띄우기       보유중이면 설명,팔기,바꾸기 띄우기       그렇지않으면 구매하기 띄우기
        if (GameManager.Instance.IsPrawnPossession(SelectedPrawn.id))  //보유중인 새우면
        {

        }
        else   //보유중이지않은 새우면
        {

        }
    }

    public void PurchasePrawn()  //새우 구입 버튼 클릭
    {
        SelectedPrawn.ClickPurchase();
        //구매하기 버튼 없애고 설명,팔기,바꾸기 띄우기
    }

    public void SellPrawn()  //새우 팔기
    {
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
}
