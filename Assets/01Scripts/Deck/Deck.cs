using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck
{
    public int index;
    public string name;
    public List<string> cards;
    public string representCard;

    public Deck()
    {
        index = 0;
        name = "";
        cards = new List<string>();
        representCard = "";
    }

    public Deck(int index, string name, List<string> cards, string representCard)
    {
        this.index = index;
        this.name = name;
        this.cards = cards;
        this.representCard = representCard;
    }

    public void SetCard(Card card, int count = 1)
    {
        //cardCount.Add(card.id, count);
        cards.Add(card.id);
    }

    public Deck DeepCopy()
    {
        Deck newDeck = new Deck();
        newDeck.name = this.name;
        newDeck.index = this.index;
        newDeck.cards = cards.ToList();
        newDeck.representCard = this.representCard;
        return newDeck;
    }

    public void Init()
    {
        //cardCount.Clear();
        cards = new List<string>();
        representCard = "";
    }

    // 수정 필요 randNum 카드 빼고 하면 범위를 넘어감
    public void Random_Represent_Card()
    {
        int randNum = Random.Range(0, cards.Count);
        representCard = cards[randNum];
    }

    public void RemoveCard(Card card)
    {
        int index = cards.FindIndex(x => x == card.id);
        cards.RemoveAt(index);
    }

    public int CardCount(string card_id)
    {
        int count = 0;
        foreach (string card in cards)
        {
            if (card == card_id)
            {
                count++;
            }
        }
        return count;
    }
}