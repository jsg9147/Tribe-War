using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DarkTonic.MasterAudio;

public class DeckEditor : MonoBehaviour
{
    public static DeckEditor instance;

    public int card_Limit_Count;

    [Header("UI")]
    public TMP_InputField deckTitleName_Input;
    public CardUI enlargeCardUI;
    public Button saveDeckButton;
    public GameObject edit_CollectCard_Content;
    public GameObject myDeckCardContent;
    public TMP_InputField searchField;

    [Header("Prefab")]
    public GameObject collectCardPrefab;
    public GameObject cardPrefab;

    public Button addDeckButton;

    List<CollectionCard> collectionList;

    List<Card> myDeckCards;
    //Dictionary<string, int> myEditCards;

    // 편집중인 덱
    public Deck myEditDeck;
    // 편집중인 내 컬렉션
    List<Card> myEditCards;

    //private void Awake()
    //{
    //    instance = this;
    //}

    //private void Start()
    //{
    //    addDeckButton.onClick.AddListener(() =>
    //    {
    //        Deck_Clear();
    //        Give_Index_of_New_Deck();
    //        Card_List_Clear();
    //        LobbyUI.instance.SelectEditMyDeck(myEditDeck);

    //        MasterAudio.PlaySound("Shuffle");
    //    });

    //    searchField.onValueChanged.AddListener((string search) =>
    //    {
    //        CardSearch(search);
    //    });
    //}

    //public void init()
    //{
    //    myEditDeck = new Deck();
    //    myEditDeck.Init();
    //    myDeckCards = new List<Card>();
    //    deckTitleName_Input.text = "";
    //}

    //void Give_Index_of_New_Deck()
    //{
    //    //var myDeckList = WebMain.instance.web.myDeckList;
    //    var myDeckList = DataManager.instance.playerDecks;
    //    int deckCount = myDeckList.Count;
    //    for (int i = 0; i <= deckCount; i++)
    //    {
    //        if (myDeckList.Exists(x => x.index == i) == false)
    //        {
    //            myEditDeck.index = i;
    //            break;
    //        }
    //    }
    //}

    //public void Input_Title(string title_Text)
    //{
    //    if (title_Text == "")
    //        saveDeckButton.interactable = false;
    //    else
    //        saveDeckButton.interactable = true;
    //}

    //public void Edit_Deck_Setup(Deck _deck)
    //{
    //    try
    //    {
    //        myEditCards = DataManager.instance.userCollection;
    //        collectionList = new List<CollectionCard>();
    //        Destroy_Card_Object_Of_Deck();

    //        myEditDeck = _deck;
    //        deckTitleName_Input.text = myEditDeck.name;

    //        Transform children = edit_CollectCard_Content.GetComponentInChildren<Transform>();

    //        foreach (Transform child in children)
    //        {
    //            if (child != edit_CollectCard_Content)
    //            {
    //                Destroy(child.gameObject);
    //            }
    //        }

    //        foreach (Card collectionCard in DataManager.instance.userCollection.Distinct())
    //        {
    //            int collectionCount = DataManager.instance.CollectionCount(collectionCard.id);
    //            collectionCount = collectionCount - myEditDeck.CardCount(collectionCard.id);
    //            if (collectionCount != 0)
    //            {
    //                GameObject cardObj = Instantiate(collectCardPrefab, edit_CollectCard_Content.transform);
    //                cardObj.GetComponent<CollectionCard>().CardSetup(collectionCard, collectionCount);
    //                collectionList.Add(cardObj.GetComponent<CollectionCard>());
    //            }
    //        }
    //        edit_CollectCard_Content.GetComponent<FlexibleGrid>().SetFlexibleGrid();

    //        foreach (string card_id in myEditDeck.cards)
    //        {
    //            Card card = DataManager.instance.CardData(card_id);
    //            var editCard = Instantiate(cardPrefab, myDeckCardContent.transform);
    //            editCard.GetComponent<CardUI>().Setup(card, true);
    //            myDeckCards.Add(card);
    //        }
    //        myDeckCardContent.GetComponent<FlexibleGrid>().SetFlexibleGrid();

    //    }
    //    catch (System.NullReferenceException ex)
    //    {
    //        print(ex);
    //    }

    //}

    //public void AddDeckButtonClick()
    //{
    //    Deck_Clear();
    //    Give_Index_of_New_Deck();
    //    Card_List_Clear();
    //    LobbyUI.instance.SelectEditMyDeck(myEditDeck);

    //    MasterAudio.PlaySound("Shuffle");
    //}
    //public void Remove_Card_Of_Deck(CardUI cardUI)
    //{
    //    Card card = cardUI.card;

    //    myEditDeck.RemoveCard(card);

    //    CollectionCard editCard = collectionList.Find(x => x.card.id == cardUI.card.id);

    //    if (editCard != null)
    //    {
    //        editCard.CardCountPlus();
    //    }
    //    else
    //    {
    //        GameObject cardObj = Instantiate(collectCardPrefab, edit_CollectCard_Content.transform);
    //        cardObj.GetComponent<CollectionCard>().CardSetup(card, 1);
    //        collectionList.Add(cardObj.GetComponent<CollectionCard>());
    //        edit_CollectCard_Content.GetComponent<FlexibleGrid>().SetFlexibleGrid();
    //    }
    //    myEditCards.Add(card);
    //    myDeckCards.Remove(card);

    //    Destroy(cardUI.gameObject);
    //}
    //public void EnlargeCard_Setup(Card card)
    //{
    //    if (!enlargeCardUI.gameObject.activeSelf)
    //    {
    //        enlargeCardUI.gameObject.SetActive(true);
    //    }
    //    enlargeCardUI.Setup(card);
    //}

    //public void Destroy_Card_Object_Of_Deck()
    //{
    //    init();
    //    Give_Index_of_New_Deck();
    //    int i = 0;
    //    GameObject[] allChildren = new GameObject[myDeckCardContent.transform.childCount];

    //    foreach (Transform child in myDeckCardContent.transform)
    //    {
    //        allChildren[i] = child.gameObject;
    //        i++;
    //    }

    //    foreach (GameObject child in allChildren)
    //    {
    //        if (child.GetComponent<Button>() == null)
    //            DestroyImmediate(child.gameObject);
    //    }
    //}

    //public void Deck_Clear()
    //{
    //    Destroy_Card_Object_Of_Deck();
    //    Edit_Deck_Setup(new Deck());
    //}

    //public void Card_Add_In_My_EditDeck(CollectionCard collectionCard)
    //{
    //    Card card = collectionCard.card;

    //    if (myEditDeck.CardCount(card.id) >= card_Limit_Count)
    //        return;

    //    myEditDeck.SetCard(card);
    //    collectionCard.CardCountMinus();

    //    if (collectionCard.cardCount <= 0)
    //    {
    //        collectionList.Remove(collectionCard);
    //        Destroy(collectionCard.gameObject);
    //    }

    //    var editCard = Instantiate(cardPrefab, myDeckCardContent.transform);
    //    editCard.GetComponent<CardUI>().Setup(card, true);
    //    myDeckCards.Add(card);

    //    myDeckCardContent.GetComponent<FlexibleGrid>().SetFlexibleGrid();
    //}
    //public void Set_Represent_Card()
    //{
    //    Card card = enlargeCardUI.card;
    //    myEditDeck.representCard = card.id;
    //}

    //public void Edit_Deck_Save()
    //{
    //    if (saveDeckButton.interactable == false)
    //        return;

    //    if (myEditDeck.representCard == "")
    //    {
    //        myEditDeck.Random_Represent_Card();
    //    }

    //    myEditDeck.name = deckTitleName_Input.text;
    //    DataManager.instance.SaveDeck(myEditDeck);

    //    deckTitleName_Input.text = "";
    //    Deck_Clear();
    //}

    //void CardSearch(string search)
    //{
    //    foreach (CollectionCard collectionCard in collectionList)
    //    {
    //        if (collectionCard.card.name.Contains(search))
    //        {
    //            collectionCard.gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            collectionCard.gameObject.SetActive(false);
    //        }
    //    }
    //    edit_CollectCard_Content.GetComponent<FlexibleGrid>().SetFlexibleGrid();
    //}

    //public void Card_List_Clear()
    //{
    //    init();
    //    Give_Index_of_New_Deck();
    //    foreach (Transform child in myDeckCardContent.transform)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //}

    //public void Deck_Clear_Button()
    //{
    //    int index = myEditDeck.index;
    //    init();
    //    Give_Index_of_New_Deck();

    //    myEditDeck.index = index;
    //    foreach (Transform child in myDeckCardContent.transform)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //}
}
