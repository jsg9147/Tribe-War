using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EffectCardLog : MonoBehaviour, IPointerDownHandler
{
    public Image cardIcon;
    public Image EffectIcon;

    Card card;

    public void SetEffectIcon(string effect)
    {
        EffectIcon.sprite = Resources.Load<Sprite>("Images/LogIcon/" + effect);
    }

    public void SetupLog(Card _card)
    {
        this.card = _card;
        cardIcon.sprite = _card.sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (EnlargeCardManager.instance != null)
            EnlargeCardManager.instance.Setup(card, true);
    }
}
