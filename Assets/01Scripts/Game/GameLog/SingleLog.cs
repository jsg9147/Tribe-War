using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SingleLog : MonoBehaviour, IPointerDownHandler
{
    public Image effectIcon;
    public Image Icon;
    public TMP_Text text;

    public Card card;

    public void SetupLog(Card effectCard, Coordinate coordinate)
    {
        this.card = effectCard;
        effectIcon.sprite = card.sprite;
        text.text = "( " + coordinate.x + ", " + coordinate.y + " )";
    }
    public void SetEffectIcon(string effect)
    {
        Icon.sprite = Resources.Load<Sprite>("Images/LogIcon/" + effect);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (EnlargeCardManager.instance != null)
            EnlargeCardManager.instance.Setup(card, true);
    }
}
