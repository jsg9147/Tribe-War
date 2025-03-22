using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyDeckScreen : MonoBehaviour
{
    [SerializeField] GameObject myDeckContent;
    public DeckManager deckManager;

    void MyDeckListUpdate()
    {
        deckManager.MyDeckListUpdate(myDeckContent, true);
    }

    private void OnEnable()
    {
        MyDeckListUpdate();
    }

    private void OnDisable()
    {
        foreach (Transform child in myDeckContent.transform)
        {
            if (child.name != "Add Deck Button")
                Destroy(child.gameObject);
        }
    }
}
