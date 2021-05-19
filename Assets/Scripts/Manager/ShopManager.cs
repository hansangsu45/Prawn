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

    public GameObject[] possessionPanel;
    public GameObject[] noPossession;
    public GameObject up, up2;
    public Text prawnExplain;
    public Text reinforceTxt;
    public Text prawnNameTxt, prawnNameTxtInDetail, prawnAbilTxt;
    public Image prawnImage;
    public Text foodCtnTxt, autoCoinLv,autoBtnTxt;

    public Animator menuAni;
    public bool bMenuOpen = false;

    private void Start()
    {
        SelectedPrawnButton = prawnBtns[0].GetComponent<Button>();
        SelectedPrawn = prawnBtns[0];
    }

    public void SelectPrawn(Button btn, ShopPrawnBtn spb)  //�������� ���� ��ư Ŭ�� ��
    {
        SelectedPrawnButton = btn;
        SelectedPrawn = spb;
        mainBtn.image.sprite = spb.spr;
        prawnNameTxt.text = spb._name;
    }

    public void PrawnDetail()  //mainBtnŬ���ϸ� ���� ������ ���� �ڼ��� ���� �г��� Ȱ��ȭ �� ���� �Լ�
    {
           //���� ���� �ɷ�ġ �ؽ�Ʈ �������
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
            string str=$"{GameManager.Instance.idToPrawn[SelectedPrawn.id].level}����";
            if(GameManager.Instance.savedData.coin>=GameManager.Instance.idToPrawn[SelectedPrawn.id].upgradePrice)
            {
                up2.SetActive(true);
                str += "\n���׷��̵� ����!";
            }
            reinforceTxt.text = str;
        }
        else
        {
            prawnAbilTxt.text = $"����:{SelectedPrawn.buyPrice}";
        }
    }

    public void PurchasePrawn()  //���� ���� ��ư Ŭ��
    {
        SelectedPrawn.ClickPurchase();
    }

    public void PurchaseSuccess()  //���� ���� ����
    {
        noPossession[0].SetActive(false);
        for (int i = 0; i < possessionPanel.Length; i++) possessionPanel[i].SetActive(true);
        GameManager.Instance.SetData();
        GameManager.Instance.ActiveSystemPanel($"{SelectedPrawn._name} ���� ����!", Color_State.GREEN);

        //prawnAbilTxt.text= ���߿�
        up.SetActive(true);
        string str = $"{GameManager.Instance.idToPrawn[SelectedPrawn.id].level}����";
        if (GameManager.Instance.savedData.coin >= GameManager.Instance.idToPrawn[SelectedPrawn.id].upgradePrice)
        {
            up2.SetActive(true);
            str += "\n���׷��̵� ����!";
        }
        reinforceTxt.text = str;
    }

    public void SellPrawn()  //���� �ȱ�
    {
        if(GameManager.Instance.savedData.currentPrawn.id==SelectedPrawn.id)  //�������� ���� �Ǹ� ����
        {
            GameManager.Instance.ActiveSystemPanel("�������� ����� �� �� �����ϴ�.", Color_State.RED,93);
            return;
        }
        if(SelectedPrawn.id==10)  //�⺻ ���� �Ǹ� ����
        {
            GameManager.Instance.ActiveSystemPanel("�ش� ����� �� �� �����ϴ�.", Color_State.RED);
            return;
        }
        GameManager.Instance.savedData.coin += GameManager.Instance.idToPrawn[SelectedPrawn.id].price;
        GameManager.Instance.SetData();
        GameManager.Instance.savedData.prawns.Remove(GameManager.Instance.idToPrawn[SelectedPrawn.id]);
        GameManager.Instance.idToPrawn.Remove(SelectedPrawn.id);

        noPossession[0].SetActive(true);
        for (int i = 0; i < possessionPanel.Length; i++) possessionPanel[i].SetActive(false);
        up.SetActive(false);
        SelectedPrawn.upgrade.SetActive(false);
        prawnAbilTxt.text = $"����:{SelectedPrawn.buyPrice}";

        GameManager.Instance.ActiveSystemPanel("�Ǹ� �Ϸ�!",Color_State.BLACK,100);
    }

    public void ChangePrawn()  //���� ��ü
    {
        GameManager.Instance.SaveData();

        Prawn p= GameManager.Instance.idToPrawn[SelectedPrawn.id];

        GameManager.Instance.savedData.currentPrawn = p;
        GameManager.Instance.myPrawn.PrawnLoad(p);
        GameManager.Instance.SetData();

        GameManager.Instance.ActiveSystemPanel(p.name + "�� ��ü�Ͽ����ϴ�.");
    }

    public void UpgradeRenewal()
    {
        for(int i=0; i<prawnBtns.Length; i++)
        {
            if(GameManager.Instance.IsPrawnPossession(prawnBtns[i].id))
            {
                if(GameManager.Instance.savedData.coin>=GameManager.Instance.idToPrawn[prawnBtns[i].id].upgradePrice && GameManager.Instance.idToPrawn[prawnBtns[i].id].level < 5)
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

    public void MenuAni(bool b)
    {
        //bMenuOpen = !bMenuOpen;
        menuAni.SetBool("menu", b);
       
    }

    public void AutoLVUp()
    {
        if(GameManager.Instance.savedData.coin>=GameManager.Instance.savedData.autoCoinUpPrice)
        {
            GameManager.Instance.savedData.coin -= GameManager.Instance.savedData.autoCoinUpPrice;
            GameManager.Instance.savedData.autocoinLv++;
            
            if(!GameManager.Instance.savedData.isAutoCoin)
            {
                GameManager.Instance.savedData.isAutoCoin = true;
                StartCoroutine(GameManager.Instance.AutoTouch());
            }
            GameManager.Instance.savedData.autoCoin = GameManager.Instance.savedData.nextAutoCoin;
            GameManager.Instance.savedData.nextAutoCoin *= 2;
            GameManager.Instance.savedData.autoCoinUpPrice *= 2;

            GameManager.Instance.SetData();
        }
        else
        {
            GameManager.Instance.ActiveSystemPanel("���� �����մϴ�.");
        }
    }
}
