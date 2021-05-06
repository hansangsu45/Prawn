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

    public GameObject[] possessionPanel;
    public GameObject[] noPossession;
    public GameObject up, up2;
    public Text prawnExplain;
    public Text reinforceTxt;
    public Text prawnNameTxt, prawnNameTxtInDetail, prawnAbilTxt;
    public Image prawnImage;

    private void Start()
    {
        SelectedPrawnButton = prawnBtns[0].GetComponent<Button>();
        SelectedPrawn = prawnBtns[0];
    }

    public void SelectPrawn(Button btn, ShopPrawnBtn spb)  //상점에서 새우 버튼 클릭 시
    {
        SelectedPrawnButton = btn;
        SelectedPrawn = spb;
        mainBtn.image.sprite = spb.spr;
        prawnNameTxt.text = spb._name;
    }

    public void PrawnDetail()  //mainBtn클릭하면 새로 나오는 새우 자세히 보기 패널을 활성화 할 때의 함수
    {
           //이제 새우 능력치 텍스트 띄워야함
        up.SetActive(false);
        up2.SetActive(false);
        bool isPoss = GameManager.Instance.IsPrawnPossession(SelectedPrawn.id);

        prawnNameTxtInDetail.text = SelectedPrawn._name;
        prawnExplain.text = SelectedPrawn.ex;
        prawnImage.sprite = SelectedPrawn.spr;

        foreach (GameObject o in possessionPanel) o.SetActive(isPoss);
        foreach (GameObject o in noPossession) o.SetActive(!isPoss);

        if (isPoss)
        {
            up.SetActive(true);
            string str=$"{GameManager.Instance.idToPrawn[SelectedPrawn.id].level}레벨";
            if(GameManager.Instance.savedData.coin>=GameManager.Instance.idToPrawn[SelectedPrawn.id].upgradePrice)
            {
                up2.SetActive(true);
                str += "\n업그레이드 가능!";
            }
            reinforceTxt.text = str;
        }
    }

    public void PurchasePrawn()  //새우 구입 버튼 클릭
    {
        SelectedPrawn.ClickPurchase();
        noPossession[0].SetActive(false);
        for (int i = 0; i < possessionPanel.Length; i++) possessionPanel[i].SetActive(true);
        GameManager.Instance.SetData();
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

        noPossession[0].SetActive(true);
        for (int i = 0; i < possessionPanel.Length; i++) possessionPanel[i].SetActive(false);
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
