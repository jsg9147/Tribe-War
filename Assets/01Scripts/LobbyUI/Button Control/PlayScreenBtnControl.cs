using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DarkTonic.MasterAudio;

public class PlayScreenBtnControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        MasterAudio.PlaySound("ButtonClick");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MasterAudio.PlaySound("BtnMouseEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
