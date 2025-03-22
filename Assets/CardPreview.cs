using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CardPreview : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text countText;
    public TMP_Text costText;

    public int count;

    public Card card;

    EnlargeCardManager enlargeCardManager;

    public void Setup(Card targetCard, int count)
    {
        card = targetCard;
        nameText.text = targetCard.name;
        countText.text = count.ToString();
        costText.text = card.cost.ToString();
        this.count = count;
    }

    public void AddCard()
    {
        count++;
        countText.text = count.ToString();
    }
    public void RemoveCard()
    {
        count--;
        countText.text = count.ToString();
    }

    public void SetEnlargeCardManager(EnlargeCardManager largeCardManager)
    {
        enlargeCardManager = largeCardManager; 
    }

    public void EnlargeCardSetup()
    {
        enlargeCardManager.Setup(card, true);
    }
}
