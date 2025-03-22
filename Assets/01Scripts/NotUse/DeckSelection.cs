using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckSelection : MonoBehaviour
{
    [SerializeField] Image deckImage;
    [SerializeField] Image selectImage;
    [SerializeField] TMP_Text deckTitle;
    [SerializeField] public Button deleteButton;

    public Deck deck;

    //삭제해도 되나?


    //private void Start()
    //{
        
    //}

    //public void DeckInfoSet(int index, string title)
    //{
    //    deck = WebMain.instance.web.myDeckList.Find(x => x.index == index);

    //    if(deck == null)
    //    {
    //        deck = new Deck();
    //    }
    //    deckTitle.text = title;
    //}

    //public void IsMatchWindow(bool isOn)
    //{
    //    deleteButton.gameObject.SetActive(!isOn);
    //}

    //public void Selected_This_Deck(bool isSelected)
    //{
    //    selectImage.gameObject.SetActive(isSelected);
    //}


    //public void DestroySelf() => Destroy(gameObject);
}
