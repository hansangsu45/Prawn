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
            gameManager.ActiveSystemPanel("최대레벨이어서 강화를 더이상 할 수 없습니다.");
            gameManager.ButtonUIClick(6);
        }

        _name.text = shopPrawnBtn._name;
        image.sprite = shopPrawnBtn.spr;
        explanation.text = shopPrawnBtn.ex;
        state.text = $"체력 {prawn.maxHp}->{prawn.maxHp + 50}\n공격력 {prawn.str}->{prawn.str + 110}\n방어력 {prawn.def + 5}\n휴식시간 {prawn.restTime}초->{prawn.restTime - 5}초\n먹이량 {prawn.foodAmount}->{prawn.foodAmount - 1}\n가격 {prawn.upgradePrice}코인";
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

            gameManager.ActiveSystemPanel("강화에 성공하였습니다.");
        }
        else
        {
            gameManager.ActiveSystemPanel("돈이 부족합니다.");
        }

        gameManager.ButtonUIClick(6);
    }
}
