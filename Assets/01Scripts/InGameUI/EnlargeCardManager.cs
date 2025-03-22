using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EnlargeCardManager : MonoBehaviour
{
    [SerializeField] GameObject enlargeCard;
    [SerializeField] CardUI cardUIScript;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            EnlargeCard_Close();
        }
    }

    public static EnlargeCardManager instance
    {
        get; private set;
    }
    void Awake() => instance = this;


    public void Setup(Card card, bool isFront)
    {
        if (isFront)
        {
            if (enlargeCard.activeSelf == false)
                enlargeCard.SetActive(true);

            cardUIScript.Setup(card);
        }
        else
        {
            enlargeCard.SetActive(false);
        }
    }


    public void EnlargeCard_Close()
    {
        if (enlargeCard.activeSelf)
            enlargeCard.SetActive(false);
    }
}
