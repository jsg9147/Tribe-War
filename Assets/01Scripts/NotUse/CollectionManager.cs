using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] DeckEditor deckEditorManager;

    [SerializeField] GameObject collectionCardPrefab;
    [SerializeField] GameObject editorCollectionContent;
    [SerializeField] GameObject collectionContent;

    [SerializeField] CardUI enlargeCardUI;

    [SerializeField] TMP_Dropdown sortBySelect;
    [SerializeField] TMP_InputField searchText;

    [SerializeField] TMP_Dropdown sortBySelectOfEditor;
    [SerializeField] TMP_InputField searchTextOfEditor;

    [SerializeField] Button resetButton;

    List<GameObject> collectionList = new List<GameObject>();

    //private void Start()
    //{
    //    //Button_Event_AddListener();
    //}

    //void Button_Event_AddListener()
    //{
    //    sortBySelect.onValueChanged.AddListener((int value) =>
    //    {
    //        SortByCardCollection(value);
    //    });

    //    sortBySelectOfEditor.onValueChanged.AddListener((int value) =>
    //    {
    //        SortByCardCollection(value);
    //    });

    //    searchText.onValueChanged.AddListener((string search) =>
    //    {
    //        CardSearch(search);
    //    });

    //    searchTextOfEditor.onValueChanged.AddListener((string search) =>
    //    {
    //        CardSearch(search);
    //    });

    //    resetButton.onClick.AddListener(() =>
    //    {
    //        ConditionReset();
    //    });
    //}
    //public void CreateCollectCard_Except_Deck()
    //{
    //    collectionList.Clear();
    //    ClearCollectCard(editorCollectionContent);
    //    foreach (var card_id in WebMain.instance.web.userItems.Keys)
    //    {
    //        int collectionCount = WebMain.instance.web.userItems[card_id];

    //        foreach (var myCardID in deckEditorManager.myEditDeck.cardCount.Keys)
    //        {
    //            if (myCardID == card_id)
    //            {
    //                collectionCount = collectionCount - deckEditorManager.myEditDeck.cardCount[card_id];
    //                break;
    //            }
    //        }

    //        if (collectionCount != 0)
    //        {
    //            Card card = DataManager.instance.CardData(card_id);
    //            GameObject cardObj = Instantiate(collectionCardPrefab, editorCollectionContent.transform);
    //            cardObj.GetComponent<CollectionCard>().CardSetup(card, collectionCount);
    //        }
    //    }
    //    editorCollectionContent.GetComponent<FlexibleGrid>().SetFlexibleGrid();
    //}

    //public void CreateCollectCard(GameObject collectionContent)
    //{
    //    collectionList.Clear();
    //    ClearCollectCard(collectionContent);
    //    foreach (var card_id in WebMain.instance.web.userItems.Keys)
    //    {
    //        for (int i = 0; i < WebMain.instance.web.userItems[card_id]; i++)
    //        {
    //            GameObject card = Instantiate(collectionCardPrefab, collectionContent.transform);
    //            card.GetComponent<EnlargeCardUI>().SetCardData(DataManager.instance.CardData(card_id));
    //            collectionList.Add(card);
    //        }
    //    }
    //}

    //public void ClearCollectCard(GameObject collectionContent)
    //{
    //    Transform children = collectionContent.GetComponentInChildren<Transform>();

    //    foreach (Transform child in children)
    //    {
    //        if (child != collectionContent)
    //        {
    //            Destroy(child.gameObject);
    //        }
    //    }
    //    sortBySelect.value = 0;
    //}

    //void SortByCardCollection(int value)
    //{
    //    switch (value)
    //    {
    //        case 0:
    //            var orderByID = collectionList.OrderBy(x => x.GetComponent<EnlargeCardUI>().card.id);
    //            foreach (var card in orderByID)
    //            {
    //                card.transform.SetAsLastSibling();
    //            }
    //            break;

    //        case 1:
    //            var orderByList = collectionList.OrderBy(x => x.GetComponent<EnlargeCardUI>().card.name);

    //            foreach (var card in orderByList)
    //            {
    //                card.transform.SetAsLastSibling();
    //            }
    //            break;

    //        case 2:
    //            var orderByDescendingList = collectionList.OrderBy(x => x.GetComponent<EnlargeCardUI>().card.name);

    //            foreach (var card in orderByDescendingList)
    //            {
    //                card.transform.SetAsFirstSibling();
    //            }
    //            break;

    //        case 3:
    //            var orderByLevel = collectionList.OrderBy(x => x.GetComponent<EnlargeCardUI>().card.cost);

    //            foreach (var card in orderByLevel)
    //            {
    //                card.transform.SetAsFirstSibling();
    //            }
    //            break;

    //        case 4:
    //            var orderByDescendingLevel = collectionList.OrderBy(x => x.GetComponent<EnlargeCardUI>().card.cost);

    //            foreach (var card in orderByDescendingLevel)
    //            {
    //                card.transform.SetAsLastSibling();
    //            }
    //            break;
    //    }
    //}

    //void Tribe_Sort()
    //{
    //    collectionList.Clear();
    //    ClearCollectCard(collectionContent);

    //}

    //void CardSearch(string search)
    //{
    //    foreach (var card in collectionList)
    //    {
    //        if (card.GetComponent<EnlargeCardUI>().card.name.Contains(search))
    //        {
    //            card.SetActive(true);
    //        }
    //        else
    //        {
    //            card.SetActive(false);
    //        }
    //    }
    //}

    //void ConditionReset()
    //{
    //    searchText.text = "";
    //    SortByCardCollection(0);
    //}

    //
    //public void Add_Excluded_Card(Card card)
    //{
    //    GameObject cardObject = Instantiate(collectionCardPrefab, editorCollectionContent.transform);
    //    enlargeCardUI.Setup(card, true);
    //    collectionList.Add(cardObject);
    //}

    //public void Remove_CollectionCard_In_CollectionList(EditCardEvent collectionCard)
    //{
    //    collectionList.Remove(collectionCard.gameObject);
    //}
}
