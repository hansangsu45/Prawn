using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reinforce : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Image image;
    [SerializeField] private Text explanation;
    [SerializeField] private Text state;

    private ShopPrawnBtn shopPrawnBtn;
    private Prawn prawn;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public void ReinforceBtnClick()
    {
        gameManager.ButtonUIClick(6);

        shopPrawnBtn = gameManager.shopManager.SelectedPrawn;
        prawn = gameManager.idToPrawn[shopPrawnBtn.id];

        if (prawn.level >= 5)
        {
            gameManager.ActiveSystemPanel("�ִ뷹���̾ ��ȭ�� ���̻� �� �� �����ϴ�.");
            gameManager.ButtonUIClick(6);
        }

        _name.text = shopPrawnBtn._name;
        image.sprite = shopPrawnBtn.spr;
        explanation.text = shopPrawnBtn.ex;
        state.text = $"ü�� {prawn.maxHp}->{prawn.maxHp + 50}\n���ݷ� {prawn.str}->{prawn.str + 110}\n���� {prawn.def + 5}\n�޽Ľð� {prawn.restTime}��->{prawn.restTime - 5}��\n���̷� {prawn.foodAmount}->{prawn.foodAmount - 1}\n���� {prawn.upgradePrice}����";
    }

    public void PrawnReinforce()
    {
        long upgradePrice = prawn.upgradePrice;

        if (gameManager.savedData.coin >= upgradePrice)
        {
            prawn.level += 1;
            gameManager.savedData.coin -= upgradePrice;

            prawn.maxHp += 50;
            prawn.hp = prawn.maxHp;
            prawn.str += 110;
            prawn.def += 5;
            prawn.restTime -= 5;
            prawn.foodAmount -= 1;

            gameManager.SetData();
            gameManager.shopManager.PrawnDetail();

            gameManager.ActiveSystemPanel("��ȭ�� �����Ͽ����ϴ�.");
        }
        else
        {
            gameManager.ActiveSystemPanel("���� �����մϴ�.");
        }

        gameManager.ButtonUIClick(6);
    }
}
