using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EnlargeCardUI : MonoBehaviour
{
    [SerializeField] GameObject attackSet;

    [SerializeField] TMP_Text cardName;
    [SerializeField] Image cardClassIcon;

    [SerializeField] TMP_Text explanationText;

    [SerializeField] TMP_Text attackText;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text tribeText;


    public Sprite RookFrame;
    public Sprite BishopFrame;
    public Sprite QueenFrame;
    public Sprite MagicFrame;
    public Sprite BackFrame;

    [SerializeField] Image cardFrame_Image;
    [SerializeField] Image classIcon_Image;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] Image cardImage;
    [SerializeField] List<GameObject> costImageObject;
    [SerializeField] TMP_Text tribeTMP;
    [SerializeField] TMP_Text cardTextTmp;
    [SerializeField] TMP_Text battlePower_TMP;

    public Card card;

    public void SetCardData(Card card)
    {
        this.card = card;
        cardName.text = card.name;
        cardImage.sprite = card.sprite;
        explanationText.text = card.text;
        cardClassIcon.sprite = card.cardType.typeIcon;
        costText.text = card.cost.ToString();
        tribeText.text = DataManager.instance.tribeStr[card.cardType.tribe];

        if (card.cardType.card_category == CardCategory.Monster)
        {
            attackSet.SetActive(true);
            attackText.text = card.GetBaseStat("bp").ToString();
        }
        else
        {
            attackSet.SetActive(false);
        }
    }
}
