using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InGameUIControl : MonoBehaviour
{
    public GameObject leftLayoutUI;
    public GameObject rightLayoutUI;
    public GameObject Deck_Panel;

    bool left_UI_On = false;
    bool right_UI_On = false;

    public void Left_UI_Move()
    {
        float width = leftLayoutUI.GetComponent<RectTransform>().rect.width;

        if (left_UI_On)
        {
            leftLayoutUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(leftLayoutUI.GetComponent<RectTransform>().anchoredPosition.x - width, 0);
            left_UI_On = false;
        }
        else
        {
            leftLayoutUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(leftLayoutUI.GetComponent<RectTransform>().anchoredPosition.x + width, 0);
            left_UI_On = true;
        }
    }

    public void Right_UI_Move()
    {
        float width = rightLayoutUI.GetComponent<RectTransform>().rect.width;

        if (right_UI_On)
        {
            rightLayoutUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(rightLayoutUI.GetComponent<RectTransform>().anchoredPosition.x + width, 0);
            right_UI_On = false;
        }
        else
        {
            rightLayoutUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(rightLayoutUI.GetComponent<RectTransform>().anchoredPosition.x - width, 0);
            right_UI_On = true;
        }
    }

    public void Deck_Screen_On()
    {
        Deck_Panel.SetActive(!Deck_Panel.activeSelf);
    }
}
