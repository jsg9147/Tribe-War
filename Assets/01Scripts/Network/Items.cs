using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;

public class Items : MonoBehaviour
{
    //Action<string> _createItemsCallback;
    //Action<string> _createDeckCallback;
    //Action<string> _createUserItemsCallback;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    //WebMain.instance.web.SetItems(this);

    //    _createDeckCallback = (jsonArrayString) =>
    //    {
    //        StartCoroutine(CreateDeckRoutine(jsonArrayString));
    //    };

    //    _createUserItemsCallback = (jsonArrayString) =>
    //    {
    //        StartCoroutine(CreateUserItemsRoutine(jsonArrayString));
    //    };
    //}

    //public void CreateItems()
    //{
    //    string userID = WebMain.instance.userInfo.UserID;
    //    //StartCoroutine(WebMain.instance.web.GetDeckItemList(userID, _createDeckCallback));
    //    StartCoroutine(WebMain.instance.web.GetUserCollection(userID, _createUserItemsCallback));
    //}

    //public IEnumerator RefreshItems(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    string userID = WebMain.instance.userInfo.UserID;
    //    StartCoroutine(WebMain.instance.web.GetUserCollection(userID, _createUserItemsCallback));
    //    //StartCoroutine(WebMain.instance.web.GetDeckItemList(userID, _createDeckCallback));
    //}


    //IEnumerator CreateDeckRoutine(string jsonArrayString)
    //{
    //    // Parsing json array string as an array
    //    int deckIndex = -1;
    //    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
    //    Dictionary<int, Dictionary<int, int>> deckIndexAndCardIndex = new Dictionary<int, Dictionary<int, int>>();
    //    if (jsonArray != null)
    //    {
    //        for (int i = 0; i < jsonArray.Count; i++)
    //        {
    //            int itemID = jsonArray[i].AsObject["itemID"];
    //            int itemCount = jsonArray[i].AsObject["itemCount"];
    //            deckIndex = jsonArray[i].AsObject["deckIndex"];


    //            if (deckIndexAndCardIndex.ContainsKey(deckIndex))
    //            {
    //                deckIndexAndCardIndex[deckIndex].Add(itemID, itemCount);
    //            }
    //            else
    //            {
    //                Dictionary<int, int> tempDeckItem = new Dictionary<int, int>();
    //                tempDeckItem.Add(itemID, itemCount);
    //                deckIndexAndCardIndex.Add(deckIndex, tempDeckItem);
    //            }
    //        }
    //        foreach (var key in deckIndexAndCardIndex.Keys)
    //        {
    //            //WebMain.Inst.web.userDeckItem.Add(key, deckIndexAndCardIndex[key]);
    //        }
    //    }
    //    yield return null;
    //}

    //IEnumerator CreateUserItemsRoutine(string jsonArrayString)
    //{
    //    // Parsing json array string as an array
    //    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
    //    if (jsonArray != null)
    //    {
    //        for (int i = 0; i < jsonArray.Count; i++)
    //        {
    //            string card_id = jsonArray[i].AsObject["card_id"];
    //            int card_count = jsonArray[i].AsObject["card_count"];

    //            print(card_id);
    //            WebMain.instance.web.userItems.Add(card_id, card_count);
    //        }
    //    }

    //    yield return null;
    //}
}
