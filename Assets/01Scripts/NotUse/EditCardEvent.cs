using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditCardEvent : MonoBehaviour, IPointerClickHandler
{
    public CardUI cardUI;
    CollectionCard collectionCard;

    public bool inCollector;

    private void Start()
    {
        Init();

        cardUI = GetComponent<CardUI>();
    }

    void Init()
    {
        if (GetComponent<CollectionCard>())
        {
            inCollector = true;
            collectionCard = GetComponent<CollectionCard>();
        }
        else
        {
            inCollector = false;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (inCollector)
            {
                DeckManager.instance.Add_Card_In_Deck(collectionCard, Input.GetKey(KeyCode.LeftShift));
                DarkTonic.MasterAudio.MasterAudio.PlaySound("AddCard");
            }
            else
            {
                DeckManager.instance.Remove_Card_Of_Deck(cardUI);
                DarkTonic.MasterAudio.MasterAudio.PlaySound("RemoveCard");
            }
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DeckManager.instance.EnlargeCard_Setup(cardUI.card);
        }
    }
}
