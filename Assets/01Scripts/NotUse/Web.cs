using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using Steamworks;

public class Web : MonoBehaviour
{
    //public Items items;

    //public int recentlyDeckIndex;
    //public List<CardPack> cardPacks = new List<CardPack>();

    ////public List<Deck> myDeckList;

    //public Dictionary<string, int> userItems = new Dictionary<string, int>();
    //public Dictionary<string, string> packInfo = new Dictionary<string, string>();

    ////Action<string> deckListCallback;
    ////Action<string> cardListOfDeckCallback;
    //Action<string> myOwnCardListCallback;
    ////Action<string> packListCallback;
    //Action<string> pack_CardList_Callback;

    //string userID, authority;
    //public float win, lose = 0;
    //public Deck selected_Deck;

    //#region URL List

    //public static string databaseServerURL = "http://192.168.55.50:80/tribe_db/";

    //string verCheckURL = "http://192.168.55.50:80";
    //string get_CardDatabase_Json_URL = databaseServerURL + "GetCardData.php";
    //string loginURL = databaseServerURL + "Login.php";
    //string get_My_Deck_Card_List_URL = databaseServerURL + "GetMyDeckCard.php";
    //string get_Pack_Info_URL = databaseServerURL + "GetPackInfo.php";
    //string get_My_Collection_URL = databaseServerURL + "GetMyCollection.php";
    //string get_Card_List_Of_Pack_URL = databaseServerURL + "GetCardListOfPack.php";
    //string save_Deck_Data_URL = databaseServerURL + "SaveDeckData.php";

    //string get_deck_info_URL = databaseServerURL + "GetDeckInfo.php";

    //string save_MyDeck_List_URL = databaseServerURL + "SaveMyDeckList.php";
    //string delete_Deck_URL = databaseServerURL + "DeleteDeck.php";

    //string sellItemURL = databaseServerURL + "SellItem.php";
    //string UpdateRecentlyUsedDeckURL = databaseServerURL + "UpdateRecentlyUsedDeck.php";


    //string SetBuyCardURL = databaseServerURL + "SetBuyCard.php";

    //#endregion
    //void Start()
    //{
    //    InitWeb();
    //}

    //void InitWeb()
    //{
    //    //myDeckList = new List<Deck>();
    //    StartCoroutine(GetVersion(verCheckURL));
    //    //deckListCallback = (jsonArrayString) =>
    //    //{
    //    //    StartCoroutine(GetUserDeckListRoutine(jsonArrayString));
    //    //};

    //    //cardListOfDeckCallback = (jsonArrayString) =>
    //    //{
    //    //    StartCoroutine(CreateCardOfDeckRoutine(jsonArrayString));
    //    //};

    //    myOwnCardListCallback = (jsonArrayString) =>
    //    {
    //        StartCoroutine(CreateUserItemsRoutine(jsonArrayString));
    //    };

    //    //packListCallback = (jsonArrayString) =>
    //    //{
    //    //    StartCoroutine(GetPackInfoRoutine(jsonArrayString));
    //    //};


    //    //pack_CardList_Callback = (jsonArrayString) =>
    //    //{
    //    //    StartCoroutine(Get_Pack_CardList_Routine(jsonArrayString));
    //    //};

    //    if (SteamManager.Initialized)
    //    {
    //        userID = SteamUser.GetSteamID().ToString();
    //    }
    //    StartCoroutine(GetCardDatabase());
    //}

    //IEnumerator GetVersion(string _uri)
    //{
    //    using (UnityWebRequest webRequest = UnityWebRequest.Get(_uri))
    //    {
    //        // Request and wait for the desired page.
    //        yield return webRequest.SendWebRequest();

    //        string[] pages = _uri.Split('/');
    //        int page = pages.Length - 1;

    //        switch (webRequest.result)
    //        {
    //            case UnityWebRequest.Result.ConnectionError:
    //            case UnityWebRequest.Result.DataProcessingError:
    //                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
    //                break;
    //            case UnityWebRequest.Result.ProtocolError:
    //                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
    //                break;
    //            case UnityWebRequest.Result.Success:
    //                Debug.Log(webRequest.downloadHandler.text); // webRequest.downloadHandler.text
    //                break;
    //        }
    //    }
    //}

    //public IEnumerator Login(string _userID)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("user_id", _userID);

    //    using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            string jsonArrayString = www.downloadHandler.text;
    //            Debug.Log(www.downloadHandler.text); // 미사용 코드 표시용
    //            //WebMain.instance.userInfo.SetCredentials(_userID); // 미사용 삭제 테스트 해봐야함

    //            if (www.downloadHandler.text.Contains("User register is successful"))
    //            {
    //                win = 0;
    //                lose = 0;
    //            }
    //            else
    //            {
    //                JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
    //                win = float.Parse(jsonArray[0].AsObject["win"]);
    //                lose = float.Parse(jsonArray[0].AsObject["lose"]);
    //                if (jsonArray[0].AsObject["recently_deck"] != null)
    //                {
    //                    recentlyDeckIndex = int.Parse(jsonArray[0].AsObject["recently_deck"]);
    //                }
    //            }

    //            //StartCoroutine(GetDeckInfo(_userID, deckListCallback));

    //            //StartCoroutine(GetDeckItemList(_userID, cardListOfDeckCallback));
    //            StartCoroutine(GetUserCollection(_userID, myOwnCardListCallback));
    //            //StartCoroutine(GetPackList(packListCallback));
    //            //StartCoroutine(Get_Pack_CardList(pack_CardList_Callback));

    //        }
    //    }
    //}

    //public IEnumerator GetCardDatabase()
    //{
    //    WWWForm form = new WWWForm();

    //    List<Card> cardDatas = new List<Card>();

    //    using (UnityWebRequest www = UnityWebRequest.Post(get_CardDatabase_Json_URL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            JSONNode json = JSON.Parse(www.downloadHandler.text);

    //            foreach (JSONNode card in json)
    //            {
    //                Card cardData = new Card(card);
    //                cardDatas.Add(cardData);
    //            }
    //            DataManager.instance.SetCardDataJson(json);
    //        }
    //    }
    //}

    //IEnumerator CreateUserItemsRoutine(string jsonArrayString)
    //{
    //    // Parsing json array string as an array
    //    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
    //    if (jsonArray != null)
    //    {
    //        for (int i = 0; i < jsonArray.Count; i++)
    //        {
    //            string card_id = jsonArray[i].AsObject["card"];
    //            int card_count = jsonArray[i].AsObject["count"];

    //            userItems.Add(card_id, card_count);
    //        }
    //    }

    //    yield return null;
    //}

    //public IEnumerator GetUserCollection(string _userID, System.Action<string> callback)
    //{
    //    WWWForm form = new WWWForm();

    //    form.AddField("user_id", _userID);

    //    using (UnityWebRequest www = UnityWebRequest.Post(get_My_Collection_URL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            string jsonArray = www.downloadHandler.text;

    //            callback(jsonArray);
    //        }
    //    }
    //}




    #region Get card list of pack
    //public IEnumerator Get_Pack_CardList(Action<string> callback)
    //{
    //    WWWForm form = new WWWForm();

    //    using (UnityWebRequest www = UnityWebRequest.Post(get_Card_List_Of_Pack_URL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            string jsonArray = www.downloadHandler.text;
    //            callback(jsonArray);
    //            //Debug.Log(www.downloadHandler.text);
    //        }
    //    }
    //}
    //IEnumerator Get_Pack_CardList_Routine(string jsonArrayString)
    //{
    //    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
    //    CardPack cardPack;
    //    if (jsonArray != null)
    //    {
    //        for (int i = 0; i < jsonArray.Count; i++)
    //        {
    //            string pack_code = jsonArray[i].AsObject["id"];
    //            int number = jsonArray[i].AsObject["number"];
    //            int rarity = jsonArray[i].AsObject["rarity"];

    //            cardPack = cardPacks.Find(x => x.GetPackCode() == pack_code);
    //            if (cardPack == null)
    //            {
    //                cardPack = new CardPack();
    //                cardPack.CardPack_Data_Setup(pack_code);
    //                cardPacks.Add(cardPack);
    //            }

    //            cardPack.AddCard(number, rarity);
    //        }
    //    }

    //    yield return null;
    //}

    //public IEnumerator Set_Buy_Card(string card_id, int count)
    //{
    //    WWWForm form = new WWWForm();

    //    form.AddField("user_id", SteamUser.GetSteamID().ToString());
    //    form.AddField("card", card_id);
    //    form.AddField("count", count);

    //    using (UnityWebRequest www = UnityWebRequest.Post(SetBuyCardURL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            Debug.Log(www.downloadHandler.text);
    //        }
    //    }
    //}

    #endregion

    //public IEnumerator GetDeckInfo(string id, System.Action<string> callback)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("user_id", id);
    //    using (UnityWebRequest www = UnityWebRequest.Post(get_deck_info_URL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            string jsonArray = www.downloadHandler.text;
    //            callback(jsonArray);
    //        }
    //    }
    //}

    //public IEnumerator Save_Edit_Deck(Deck deck)
    //{
    //    WWWForm form;
    //    var user_id = SteamUser.GetSteamID().ToString();
    //    int deck_index = deck.index;

    //    form = new WWWForm();

    //    form.AddField("user_id", SteamUser.GetSteamID().ToString());
    //    form.AddField("deck_index", deck_index);
    //    form.AddField("deck_name", deck.name);
    //    form.AddField("represent_card", deck.representCard);

    //    using (UnityWebRequest www = UnityWebRequest.Post(save_MyDeck_List_URL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            Debug.Log(www.downloadHandler.text);

    //            foreach (var card_ID in deck.cardCount.Keys)
    //            {
    //                form = new WWWForm();
    //                form.AddField("user_id", user_id);
    //                form.AddField("deck_index", deck_index);
    //                form.AddField("card", card_ID);
    //                form.AddField("count", deck.cardCount[card_ID]);

    //                using (UnityWebRequest www1 = UnityWebRequest.Post(save_Deck_Data_URL, form))
    //                {
    //                    yield return www1.SendWebRequest();

    //                    if (www1.result != UnityWebRequest.Result.Success)
    //                    {
    //                        Debug.Log(www1.error);
    //                    }
    //                    else
    //                    {
    //                        Debug.Log(www1.downloadHandler.text);
    //                    }
    //                }
    //            }
    //        }
    //    }


    //}

    #region Get my deck's card list
    //public IEnumerator GetDeckItemList(string _userID, System.Action<string> callback)
    //{
    //    WWWForm form = new WWWForm();

    //    form.AddField("user_id", _userID);

    //    using (UnityWebRequest www = UnityWebRequest.Post(get_My_Deck_Card_List_URL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            string jsonArray = www.downloadHandler.text;

    //            callback(jsonArray);
    //        }
    //    }
    //}

    //IEnumerator CreateCardOfDeckRoutine(string jsonArrayString)
    //{
    //    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
    //    if (jsonArray != null)
    //    {
    //        for (int i = 0; i < jsonArray.Count; i++)
    //        {
    //            int deck_index = jsonArray[i].AsObject["deck_index"];
    //            string card_id = jsonArray[i].AsObject["card"];
    //            int card_count = jsonArray[i].AsObject["count"];

    //            if (myDeckList.Exists(x => x.index == deck_index))
    //            {
    //                var deck = myDeckList.Find(x => x.index == deck_index);
    //                deck.SetCard(DataManager.instance.CardData(card_id), card_count);
    //            }
    //            else
    //            {
    //                Deck deck = new Deck();
    //                deck.index = deck_index;
    //                deck.SetCard(DataManager.instance.CardData(card_id), card_count);
    //                myDeckList.Add(deck);
    //            }
    //        }
    //    }
    //    yield return null;
    //}

    #endregion

    //public IEnumerator GetPackList(Action<string> callback)
    //{
    //    WWWForm form = new WWWForm();

    //    using (UnityWebRequest www = UnityWebRequest.Post(get_Pack_Info_URL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            string jsonArray = www.downloadHandler.text;
    //            callback(jsonArray);
    //            //Debug.Log(jsonArray);
    //        }
    //    }
    //}

    //IEnumerator GetUserDeckListRoutine(string jsonArrayString)
    //{
    //    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
        
    //    if (jsonArray != null)
    //    {
    //        for (int i = 0; i < jsonArray.Count; i++)
    //        {
    //            bool exist = false;
    //            int deck_Index = jsonArray[i].AsObject["deck_index"];
    //            string deck_title = jsonArray[i].AsObject["name"];
    //            string representCard = jsonArray[i].AsObject["represent_card"];

    //            Deck deck = new Deck();

    //            if (myDeckList.Exists(x => x.index == deck_Index))
    //            {
    //                deck = myDeckList.Find(x => x.index == deck_Index);
    //                exist = true;
    //            }

    //            deck.name = deck_title;
    //            deck.index = deck_Index;

    //            if (representCard != null)
    //            {
    //                deck.representCard = representCard;
    //            }

    //            if(exist == false)
    //                myDeckList.Add(deck);
    //        }
    //    }
    //    yield return null;
    //}

    //IEnumerator GetPackInfoRoutine(string jsonArrayString)
    //{
    //    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
    //    if (jsonArray != null)
    //    {
    //        for (int i = 0; i < jsonArray.Count; i++)
    //        {
    //            string pack = jsonArray[i].AsObject["id"];
    //            string name = jsonArray[i].AsObject["name"];

    //            packInfo.Add(pack, name);
    //        }
    //    }

    //    yield return null;
    //}

    //public IEnumerator DeleteDeck(string _deckIndex)
    //{
    //    WWWForm form = new WWWForm();

    //    form.AddField("user_id", userID);
    //    form.AddField("deck_index", _deckIndex);

    //    using (UnityWebRequest www = UnityWebRequest.Post(delete_Deck_URL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            Debug.Log(www.downloadHandler.text);
    //        }
    //    }
    //}

    //public IEnumerator UpdateRecentlyUsedDeck(string user_id, int deck_index)
    //{
    //    recentlyDeckIndex = deck_index;

    //    WWWForm form = new WWWForm();
    //    form.AddField("user_id", user_id);
    //    form.AddField("recently_use_deck", deck_index);

    //    using (UnityWebRequest www = UnityWebRequest.Post(UpdateRecentlyUsedDeckURL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {

    //        }
    //    }
    //}



    //// https://www.youtube.com/watch?v=3K9_S6RPHYA&list=PLTm4FjoXO7nfn0jB0Ig6UbZU1pUHSLhRU&index=14 
    //public IEnumerator SellItem(string _itemID, string _userID)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("user_id", _userID);
    //    form.AddField("card_id", _itemID);

    //    using (UnityWebRequest www = UnityWebRequest.Post(sellItemURL, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            Debug.Log(www.downloadHandler.text);
    //        }
    //    }
    //}

    //public void SetItems(Items items)
    //{
    //    this.items = items;
    //}
}
