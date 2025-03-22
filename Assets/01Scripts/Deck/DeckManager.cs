using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;
using TMPro;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    [SerializeField] int LeastDeckCount;
    [SerializeField] int maximumDeckCount;
    [SerializeField] int card_Limit_Count;

    List<CollectionCard> collectionCards;
    List<CardUI> editDeckCardsUI;
    int collectionCount = 0;

    [SerializeField] GameObject match_Deck_Select_Content;
    [SerializeField] GameObject myDeckList_Content; // 초기화 시킬 방법이 안떠올라 사용

    public GameObject myDeckPrefab;
    public GameObject myDeckEditPrefab;

    List<MyDeck> myDeckList;
    public Button matchButton;

    public Deck myEditDeck;

    // 나중에 한곳에 모아서 옮기기
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

    [Header("Text")]
    public TMP_Dropdown category_dropdown;
    public TMP_Text save_text;
    public TMP_Text cardSearchTitle_text;
    public TMP_Text search_text;
    public TMP_Text representCard_text;
    public TMP_Text searchInput_text;

    [Header("Deck EditScreen text")]
    public TMP_Text editDeckCount_text;
    public TMP_Text mainDeckText;
    public TMP_Text tooltip_text;
    public TMP_Text deckNotice_text;

    string cardName, cardLevel, cardTribe;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        init();
    }

    public void init()
    {
        myEditDeck = new Deck();
        myEditDeck.Init();
        deckTitleName_Input.text = "";

        editDeckCardsUI = new List<CardUI>();

        searchField.onValueChanged.AddListener((string search) =>
        {
            CardSearch(search);
        });

        category_dropdown.onValueChanged.AddListener((int index) =>
        {
            CardSearch(searchField.text);
        });

        CardEntitySetup();
    }

    void CardEntitySetup()
    {   
        collectionCards = new List<CollectionCard>();

        foreach (Card collectionCard in DataManager.instance.cardDatas)
        {
            int cardCount = DataManager.instance.userCollection[collectionCard];
            GameObject cardObj = Instantiate(collectCardPrefab, edit_CollectCard_Content.transform);
            cardObj.GetComponent<CollectionCard>().CardSetup(collectionCard, cardCount);
            collectionCards.Add(cardObj.GetComponent<CollectionCard>());
            cardObj.SetActive(false);
        }

        for (int i = 0; i < maximumDeckCount; i++)
        {
            GameObject editCard = Instantiate(cardPrefab, myDeckCardContent.transform);
            editDeckCardsUI.Add(editCard.GetComponent<CardUI>());
            editCard.gameObject.SetActive(false);   
        }
    }

    void Give_Index_of_New_Deck()
    {
        //var myDeckList = WebMain.instance.web.myDeckList;
        List<Deck> myDeckList = DataManager.instance.playerDecks;
        int deckCount = myDeckList.Count;

        for (int i = 0; i <= deckCount; i++)
        {
            if (myDeckList.Exists(x => x.index == i) == false)
            {
                myEditDeck.index = i;
                break;
            }
        }
    }

    public void AddDeckButton()
    {
        Card_List_Clear();
        LobbyUI.instance.SelectEditMyDeck(myEditDeck);
        MasterAudio.PlaySound("Shuffle");
    }

    public void Input_Title(string title_Text)
    {
        if (title_Text == "")
        {
            saveDeckButton.interactable = false;
        }
        else
        {
            saveDeckButton.interactable = true;
        }
    }

    public void Edit_Deck_Setup(Deck _deck)
    {
        try
        {
            myEditDeck = _deck.DeepCopy();
            deckTitleName_Input.text = myEditDeck.name;

            Transform children = edit_CollectCard_Content.GetComponentInChildren<Transform>();

            foreach (Transform child in children)
            {
                if (child != edit_CollectCard_Content)
                {
                    child.gameObject.SetActive(false);
                }
            }

            foreach (Card userCard in DataManager.instance.userCollection.Keys)
            {
                int cardCount = 0; 
                cardCount = DataManager.instance.userCollection[userCard] - myEditDeck.CardCount(userCard.id);

                CollectionCard collectionCard = collectionCards.Find(x => x.card.id == userCard.id);
                collectionCard.cardCount = cardCount;
                if (collectionCard.cardCount > 0)
                {
                    collectionCard.gameObject.SetActive(true);
                    collectionCard.CountUpdate();
                    collectionCount++;
                }
                else
                {
                    collectionCard.gameObject.SetActive(false);
                }
            }
            for (int index = 0; index < myEditDeck.cards.Count; index++)
            {
                Card card = DataManager.instance.CardData(myEditDeck.cards[index]);
                editDeckCardsUI[index].gameObject.SetActive(true);
                editDeckCardsUI[index].Setup(card);
            }

            edit_CollectCard_Content.GetComponent<FlexibleGrid>().SetFlexibleGrid(collectionCount);
            editDeckCount_text.text = myEditDeck.cards.Count.ToString();
            myDeckCardContent.GetComponent<FlexibleGrid>().SetFlexibleGrid(myEditDeck.cards.Count);

            MyDeckListUpdate(myDeckList_Content, true);
        }
        catch (System.NullReferenceException ex)
        {
            print(ex);
        }

    }

    public void AddDeckButtonClick()
    {
        Deck_Clear();
        Give_Index_of_New_Deck();
        Card_List_Clear();
        LobbyUI.instance.SelectEditMyDeck(myEditDeck);
        Edit_Deck_Setup(myEditDeck);
        MasterAudio.PlaySound("Shuffle");
    }

    public void Add_Card_In_Deck(CollectionCard collectionCard, bool isMax)
    {
        Card card = collectionCard.card;
        int addCount = 1;
        if (myEditDeck.CardCount(card.id) >= card_Limit_Count)
        {
            return;
        }

        if (isMax)
        {
            addCount = collectionCard.cardCount;
        }

        for (int i = 0; i < addCount; i++)
        {
            if (myEditDeck.cards.Count >= maximumDeckCount)
            {
                break;
            }

            myEditDeck.SetCard(card);
            collectionCard.CardCountMinus();

            editDeckCardsUI[myEditDeck.cards.Count - 1].Setup(card, true);
            editDeckCardsUI[myEditDeck.cards.Count - 1].gameObject.SetActive(true);

        }

        if (collectionCard.cardCount <= 0)
        {
            collectionCard.gameObject.SetActive(false);
            collectionCount--;
        }

        editDeckCount_text.text = myEditDeck.cards.Count.ToString();
        myDeckCardContent.GetComponent<FlexibleGrid>().SetFlexibleGrid(myEditDeck.cards.Count);
    }

    public void Remove_Card_Of_Deck(CardUI cardUI)
    {
        myEditDeck.RemoveCard(cardUI.card);
        cardUI.gameObject.SetActive(false);

        CollectionCard editCard = collectionCards.Find(x => x.card.id == cardUI.card.id);
        if (editCard.cardCount <= 0)
        {
            collectionCount++;
            editCard.gameObject.SetActive(true);
            edit_CollectCard_Content.GetComponent<FlexibleGrid>().SetFlexibleGrid(collectionCount);
        }

        editCard.CardCountPlus();
        editDeckCount_text.text = myEditDeck.cards.Count.ToString();
    }
    public void EnlargeCard_Setup(Card card)
    {
        if (!enlargeCardUI.gameObject.activeSelf)
        {
            enlargeCardUI.gameObject.SetActive(true);
        }
        enlargeCardUI.Setup(card);
    }

    public void Deck_Clear()
    {
        foreach (CardUI cardUI in editDeckCardsUI)
        {
            cardUI.gameObject.SetActive(false);
        }
        Edit_Deck_Setup(new Deck());
    }
    public void Set_Represent_Card()
    {
        Card card = enlargeCardUI.card;
        myEditDeck.representCard = card.id;
    }

    public void Edit_Deck_Save()
    {
        if (saveDeckButton.interactable == false) { return; }

        if(myEditDeck.cards.Count < LeastDeckCount) 
        {
            LobbyUI.instance.DeckNoticeWindow();
            return; 
        }
        myEditDeck.name = deckTitleName_Input.text;
        deckTitleName_Input.text = "";

        if (myEditDeck.representCard == "")
        {
            myEditDeck.Random_Represent_Card();
        }

        LobbyUI.instance.DeckEditorButtonClick();
        DataManager.instance.SaveDeck(myEditDeck);

        Deck_Clear();
    }

    public void CardSearch(string search)
    {
        TMP_Dropdown.OptionData searchCondition = category_dropdown.options[category_dropdown.value];
        bool contain = false;
        foreach (CollectionCard collectionCard in collectionCards)
        {
            if (searchCondition.text == cardName)
            {
                contain = collectionCard.card.name.Contains(search);
            }
            else if (searchCondition.text == cardLevel)
            {
                int level = collectionCard.card.cost + 1;
                contain = level.ToString().Contains(search);
            }
            else if (searchCondition.text == cardTribe)
            {
                contain = DataManager.instance.tribeStr[collectionCard.card.cardType.tribe].Contains(search);
            }
            collectionCard.gameObject.SetActive(contain);
        }
        edit_CollectCard_Content.GetComponent<FlexibleGrid>().SetFlexibleGrid();
    }
    public void Card_List_Clear()
    {
        myEditDeck = new Deck();
        myEditDeck.Init();
        deckTitleName_Input.text = "";
        Give_Index_of_New_Deck();
        Deck_Clear();
    }

    public void Deck_Clear_Button()
    {
        int index = myEditDeck.index;
        myEditDeck.index = index;

        Deck_Clear();
    }

    public void MyDeckListUpdate(GameObject myDeckContent, bool _EditMyDeck)
    {
        if (DataManager.instance.playerDecks == null)
            return;

        foreach (Transform child in myDeckContent.transform)
        {
            Destroy(child.gameObject);
        }

        if (myDeckContent.transform.name == "MyDeckContent")
        {
            Button addDeckBtn = Instantiate(addDeckButton, myDeckContent.transform);
            addDeckBtn.onClick.AddListener(() =>
            {
                AddDeckButton();
            });
        }

        myDeckList = new List<MyDeck>();
        List<Deck> deckList = DataManager.instance.playerDecks;

        foreach (Deck deck in deckList)
        {
            if (_EditMyDeck)
            {
                EditMyDeckCreate(myDeckContent, deck.DeepCopy()) ;
                myDeckContent.GetComponent<FlexibleGrid>().SetFlexibleGrid();
            }
            else
            {
                MyDeckCreate(myDeckContent, deck.DeepCopy());
                myDeckContent.GetComponent<FlexibleGrid>().SetFlexibleGrid();
                matchButton.interactable = !(DataManager.instance.Select_Deck == null);
            }
        }

    }

    void MyDeckCreate(GameObject myDeckContent, Deck _deck)
    {
        GameObject myDeckObj = Instantiate(myDeckPrefab, myDeckContent.transform);
        MyDeck myDeck = myDeckObj.GetComponent<MyDeck>();
        myDeck.Setup(_deck.DeepCopy());

        if (DataManager.instance.userInfo.Resently_Deck_index == _deck.index)
        {
            myDeck.SelectDeck(true);
            DataManager.instance.Select_Deck = _deck;
        }

        myDeckList.Add(myDeck);

        myDeckObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            foreach (MyDeck deck in myDeckList)
            {
                deck.SelectDeck(false);
            }
            myDeck.SelectDeck(true);
            DataManager.instance.userInfo.Resently_Deck_index = _deck.index;
            DataManager.instance.Select_Deck = _deck;

            matchButton.interactable = true;
        });
    }

    void EditMyDeckCreate(GameObject myDeckContent, Deck deck)
    {
        GameObject myDeckObj = Instantiate(myDeckEditPrefab, myDeckContent.transform);
        MyEditDeck myDeck = myDeckObj.GetComponent<MyEditDeck>();

        myDeck.MyDeckInfoSetup(deck.DeepCopy());

        myDeckObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            Deck copyDeck = deck.DeepCopy();
            LobbyUI.instance.SelectEditMyDeck(copyDeck);
            MasterAudio.PlaySound("Shuffle");
        });

        myDeck.deleteButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Deck deck = DataManager.instance.playerDecks.Find(x => x.index == myDeck.deck.index);
            DataManager.instance.playerDecks.Remove(deck);
            DataManager.instance.SaveDeckList();
            MyDeckListUpdate(myDeckList_Content, true);
        });
    }

    public void ChangeLanguage()
    {
        LocalizationData localizationData = LocalizationManager.instance.UIText;
        category_dropdown.options.Clear();
        for (int i = 0; i < localizationData.items.Count; i++)
        {
            if (localizationData.items[i].tag == "Save")
            {
                save_text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CardSearchTitle")
            {
                cardSearchTitle_text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "Search")
            {
                search_text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "RepresentCard")
            {
                representCard_text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "SearchInput")
            {
                searchInput_text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "MainDeck")
            {
                mainDeckText.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "DeckEditTooltip")
            {
                tooltip_text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "DeckNotice")
            {
                deckNotice_text.text = localizationData.items[i].value;
            }

            if (localizationData.items[i].tag == "CategoryName")
            {
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(localizationData.items[i].value);
                category_dropdown.options.Add(optionData);
                cardName = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CategoryLevel")
            {
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(localizationData.items[i].value);
                category_dropdown.options.Add(optionData);
                cardLevel = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CategoryTribe")
            {
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(localizationData.items[i].value);
                category_dropdown.options.Add(optionData);
                cardTribe = localizationData.items[i].value;
            }
        }
        category_dropdown.value = 0;

    }
}
