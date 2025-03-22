using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextClipCopy : MonoBehaviour, IPointerClickHandler
{
    TMP_Text text;

    public static string Clipboard
    {
        get { return GUIUtility.systemCopyBuffer; }
        set { GUIUtility.systemCopyBuffer = value; }
    }

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Clipboard = text.text;
        Debug.Log(Clipboard);
    }
}
