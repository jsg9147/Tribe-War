using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyEditDeck : MonoBehaviour
{
    public Deck deck;
    public CardUI card_UI;
    public TMP_Text title_text;
    public Button deleteButton;

    public void MyDeckInfoSetup(Deck _deck)
    {
        deck = _deck;
        Card represent = DataManager.instance.CardData(deck.representCard);

        title_text.text = deck.name;

        card_UI.Setup(represent);
    }
}
