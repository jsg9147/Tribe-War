using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayLog : MonoBehaviour
{
    public Image cardImage;
    public TMP_Text nameTMP;
    public TMP_Text coordinateTMP;
    public TMP_Text effectTMP;



    public void Monster_Log(Entity fieldCard, string infoStr = "소환!")
    {
        nameTMP.text = fieldCard.card.name;
        cardImage.sprite = fieldCard.card.sprite;
        effectTMP.text = infoStr;
        coordinateTMP.text = "(" + fieldCard.coordinate.x + ", " + fieldCard.coordinate.y + ")";
    }

    public void Monster_Move_Log(Entity fieldCard, Coordinate before_Coord)
    {
        nameTMP.text = fieldCard.card.name;
        cardImage.sprite = fieldCard.card.sprite;
        coordinateTMP.text = "(" + before_Coord.x + ", " + before_Coord.y + ")";
        effectTMP.text = "이동 (" + fieldCard.coordinate.x + ", " + fieldCard.coordinate.y + ")";
    }

    public void Magic_Log(Card card, string infoStr = "마법카드 발동!")
    {
        nameTMP.text = card.name;
        cardImage.sprite = card.sprite;
        effectTMP.text = infoStr;
        coordinateTMP.text = "";
    }

    public void Effected_Log(Entity fieldCard, string infoStr = "효과 발동")
    {
        nameTMP.text = fieldCard.card.name;
        cardImage.sprite = fieldCard.card.sprite;
        effectTMP.text = infoStr;
        coordinateTMP.text = "(" + fieldCard.coordinate.x + ", " + fieldCard.coordinate.y + ")";
    }

    public void Monster_Battle_Log(Entity card, int beforeBP, int afterBP, string effectStr = "전투")
    {
        nameTMP.text = card.card.name;
        cardImage.sprite = card.card.sprite;
        coordinateTMP.text = "(" + card.coordinate.x + ", " + card.coordinate.y + ")";
        if (afterBP == 0)
            effectTMP.text = effectStr + " " + beforeBP + "파괴!";
        else
            effectTMP.text = effectStr + " " + beforeBP + "데미지 " + afterBP;
    }

    public void Outpost_Attack_Log(Entity fieldCard)
    {
        nameTMP.text = fieldCard.card.name;
        cardImage.sprite = fieldCard.card.sprite;
        coordinateTMP.text = "(" + fieldCard.coordinate.x + ", " + fieldCard.coordinate.y + ")";
        effectTMP.text = "거점 공격! (" + fieldCard.card.GetEffectiveValue("bp") + ")";
    }

    public void Outpost_Damaged_Log(Outpost outpost, Entity fieldCard)
    {
        nameTMP.text = fieldCard.card.name;
        cardImage.sprite = fieldCard.card.sprite;
        coordinateTMP.text = "(" + outpost.coordinate.x + ", " + outpost.coordinate.y + ")";
        effectTMP.text = "뭐지! (" + (outpost.life + fieldCard.GetEffectiveValue("bp")) + "기억안나" + outpost.life + ")";
    }
}
