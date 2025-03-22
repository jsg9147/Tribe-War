using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPackBuy : MonoBehaviour
{
    public ShopScreen shopScreen;

    CardPack cardPack;

    public CardUI cardPrefab;

    public Image background;
    public Image pack_Image;

    public TMP_Text nameText;

    public int card_Count;

    public float nomal_Importance;
    public float rare_Importance;
    public float unique_Importance;

    float all_Importance;

    //public class CardSlot
    //{
    //    public List<Card> slot = new List<Card>();
    //}
    //public CardSlot[] displaySlot;

    //public void Pack_Setup(CardPack pack, string pack_name, ShopScreen _shop)
    //{
    //    nameText.text = pack_name;
    //    pack_Image.sprite = Resources.Load<Sprite>("Images/Pack/" + pack.GetPackCode());

    //    all_Importance = nomal_Importance + rare_Importance + unique_Importance;

    //    this.cardPack = pack;
    //    this.shopScreen = _shop;
    //}

    //public List<int> Random_One_Pack()
    //{
    //    List<int> cards = new List<int>();

    //    for (int i = 0; i < card_Count; i++)
    //    {
    //        float random_importance = Random.Range(0f, all_Importance);
    //        if (random_importance < nomal_Importance)
    //        {
    //            cards.Add(cardPack.GetRandomNomal());
    //        }
    //        else if (random_importance > nomal_Importance && random_importance < nomal_Importance + rare_Importance)
    //        {
    //            cards.Add(cardPack.GetRandomRare());
    //        }
    //        else if (random_importance < nomal_Importance + rare_Importance + unique_Importance && random_importance > nomal_Importance + rare_Importance)
    //        {
    //            cards.Add(cardPack.GetRandomUnique());
    //        }
    //    }

    //    return cards;
    //}

    //public void DrawCastPack()
    //{
    //    // 상점기능이 사라져 버렸음
    //    //shopScreen.DrawScreen_On();
    //    List<int> pack = Random_One_Pack();
    //    Dictionary<string, int> cardCount = new Dictionary<string, int>();

    //    for (int i = 0; i < shopScreen.SlotObject.Length; i++)
    //    {
    //        int randCycle = Random.Range(3, 7);
    //        int getIndex = Random.Range(0, 9);
    //        CardUI cardUI;
    //        Card card;
    //        for (int j = 0; j < 10; j++)
    //        {
    //            if (j == getIndex)
    //            {
    //                cardUI = Instantiate(cardPrefab, shopScreen.SlotObject[i].transform);
    //                card = GetCard(pack[i]);
    //                cardUI.Setup(card, true);
    //                if (cardCount.ContainsKey(card.id))
    //                {
    //                    cardCount[card.id] = cardCount[card.id] + 1;
    //                }
    //                else
    //                {
    //                    cardCount.Add(card.id, 1);
    //                }
    //            }
    //            else
    //            {
    //                CardUI tempCard = Instantiate(cardPrefab, shopScreen.SlotObject[i].transform);
    //                tempCard.Setup(GetRandom_Recorded_Card());
    //            }

    //        }
    //        StartCoroutine(shopScreen.StartSlot(i, randCycle, getIndex));
    //    }

    //    //foreach (string card_id in cardCount.Keys)
    //    //{
    //    //    StartCoroutine(WebMain.instance.web.Set_Buy_Card(card_id, cardCount[card_id]));
    //    //}
    //}

    //Card GetCard(int card_number)
    //{
    //    string card_code;
    //    string card_numStr = string.Format("{0:D3}", card_number);

    //    card_code = cardPack.GetPackCode() + "-" + card_numStr;
    //    return DataManager.instance.CardData(card_code);
    //}

    //Card GetRandom_Recorded_Card()
    //{
    //    int random = Random.Range(1, card_Count);
    //    string card_code;
    //    string card_numStr = string.Format("{0:D3}", random);

    //    card_code = cardPack.GetPackCode() + "-" + card_numStr;
    //    return DataManager.instance.CardData(card_code);
    //}
}
