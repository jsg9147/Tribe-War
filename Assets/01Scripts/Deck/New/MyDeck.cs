using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyDeck : MonoBehaviour
{
    public GameObject select_Background;
    public TMP_Text myDeckTitle;
    public Image representCardImage;

    Card representCard;
    Deck myDeck;

    public bool select;

    public void Setup(Deck deck)
    {
        myDeck = deck;
        representCard = DataManager.instance.CardData(myDeck.representCard);
        myDeckTitle.text = deck.name;

        if (representCard != null)
            representCardImage.sprite = representCard.sprite;

    }

    public void SelectDeck(bool selected)
    {
        select_Background.SetActive(selected);
        select = selected;
    }
}
