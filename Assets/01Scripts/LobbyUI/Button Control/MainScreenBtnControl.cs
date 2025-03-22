using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DarkTonic.MasterAudio;

public class MainScreenBtnControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    Image btnEffect;

    void Start()
    {
        btnEffect =  transform.GetChild(0).GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MasterAudio.PlaySound("BtnMouseEnter");
        btnEffect.color = new Color(1, 1, 1, 0.4f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btnEffect.color = new Color(1, 1, 1, 0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MasterAudio.PlaySound("ButtonClick");
    }
}
