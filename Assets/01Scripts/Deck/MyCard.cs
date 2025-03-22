using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyCard : MonoBehaviour
{
    [SerializeField] Image cardImage;
    [SerializeField] TMP_Text cardCost;
    [SerializeField] TMP_Text cardTitle;
    [SerializeField] TMP_Text cardCount;

    public Card card;

    public void SetMyCard(Card card, int count)
    {
        this.card = card;
        cardImage.sprite = card.sprite;
        cardCost.text = card.cost.ToString();
        cardCount.text = count.ToString();
        cardTitle.text = card.name;
    }

    public void MyCardUpdate(int count)
    {
        cardCount.text = count.ToString();
    }
}
