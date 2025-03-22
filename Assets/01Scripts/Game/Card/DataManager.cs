using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using Newtonsoft.Json;
using Steamworks;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    JSONNode cardJson;
    public Dictionary<int, string> tutorial_cards;
    public Dictionary<string, int> ai_Deck_Dictionary;
    public Dictionary<Tribe, string> tribeStr;
    public int ai_Deck_Count;

    public List<Card> cardDatas;
    public Dictionary<Card, int> userCollection;

    public List<Deck> playerDecks;
    public List<Deck> ai_Decks;


    public int lastUseDeck;

    public UserInfo userInfo;
    public ulong steamID;

    public Deck Select_Deck;

    public List<Card> selected_Deck_Cards;
    public Language language;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        tutorial_cards = new Dictionary<int, string>();

        steamID = SteamUser.GetSteamID().m_SteamID;
        cardDatas = new List<Card>();
        Select_Deck = new Deck();
        userInfo = new UserInfo();
        userCollection = new Dictionary<Card, int>();
        playerDecks = new List<Deck>();
        selected_Deck_Cards = new List<Card>();

        ai_Deck_Dictionary = new Dictionary<string, int>();

        LanguageSet();

        TextAsset textData = Resources.Load("CardJson/CardData") as TextAsset;
        JSONNode nodeData = JSON.Parse(textData.text);

        SetCardDataJson(nodeData);

        Set_CardData();
        TribeStrSet();
        Load_Tutorial_Card();
        Load_User_Data();
        Load_Deck();
        Load_AI_Deck();
    }
    void Set_CardData()
    {
        cardDatas.Clear();
        foreach (JSONNode card in cardJson)
        {
            Card cardData = new Card(card, language);
            cardDatas.Add(cardData);
        }
        Temp_Set_Collection();
    }

    public void TribeStrSet()
    {
        tribeStr = new Dictionary<Tribe, string>();
        LocalizationData localizationData = LocalizationManager.instance.Read("LocalizationData/Tribe");
        foreach (LocalizationItem data in localizationData.items)
        {
            if (data.tag == "Warrior")
            {
                tribeStr.Add(Tribe.Warrior, data.value);
            }
            if (data.tag == "Magician")
            {
                tribeStr.Add(Tribe.Magician, data.value);
            }
            if (data.tag == "Dragon")
            {
                tribeStr.Add(Tribe.Dragon, data.value);
            }
            if (data.tag == "Common")
            {
                tribeStr.Add(Tribe.Common, data.value);
            }

        }
    }


    //임시 콜렉션 풀카드로 만들어주는 함수
    void Temp_Set_Collection()
    {
        foreach (Card card in cardDatas)
        {
            userCollection.Add(card, 4);
        }
    }

    public void SetCardDataJson(JSONNode json) => cardJson = json;

    public Card Tutorial_Card(int index)
    {
        return CardData(tutorial_cards[index]);
    }

    public Card CardData(string card_id)
    {
        return cardDatas.Find(x => x.id == card_id);
    }

    void Load_Tutorial_Card()
    {
        TextAsset textAsset = Resources.Load("CardJson/TutorialCard") as TextAsset;

        JSONNode root = JSON.Parse(textAsset.text);
        for (int i = 0; i < root.Count; i++)
        {
            JSONNode itemData = root[i];
            tutorial_cards.Add(Int32.Parse(itemData["index"].Value), itemData["id"].Value);
        }
    }

    public List<Card> AI_CardList()
    {
        int index = UnityEngine.Random.Range(0, ai_Decks.Count);
        List<Card> ai_Cards = new List<Card>();

        Deck AI_PlayDeck = ai_Decks[index];

        foreach (string card_id in AI_PlayDeck.cards)
        {
            ai_Cards.Add(CardData(card_id));
        }

        return ai_Cards;
    }

    void Load_AI_Deck()
    {
        ai_Decks = new List<Deck>();
        int randomIndex = UnityEngine.Random.Range(0, ai_Deck_Count);

        string deckFile_name = "CardJson/AIDeck" + randomIndex.ToString();

        Deck ai_Deck = new Deck();

        TextAsset textAsset = Resources.Load(deckFile_name) as TextAsset;

        JSONNode root = JSON.Parse(textAsset.text);
        for (int i = 0; i < root.Count; i++)
        {
            JSONNode itemData = root[i];
            ai_Deck_Dictionary.Add(itemData["id"].Value, Int32.Parse(itemData["count"].Value));
        }

        foreach (string card_Id in ai_Deck_Dictionary.Keys)
        {
            for (int i = 0; i < ai_Deck_Dictionary[card_Id]; i++)
            {
                ai_Deck.SetCard(CardData(card_Id));
            }
        }

        ai_Decks.Add(ai_Deck);
    }

    public void Load_User_Data()
    {
        if (ES3.KeyExists(steamID.ToString()))
        {
            string json = ES3.Load<string>(steamID.ToString());
            userInfo = JsonUtility.FromJson<UserInfo>(json);
        }
        else
        {
            SaveUserInfo();
            string json = ES3.Load<string>(steamID.ToString());
            userInfo = JsonUtility.FromJson<UserInfo>(json);
        }
    }
    public void SaveDeck(Deck deck)
    {
        if (playerDecks.Exists(x => x.index == deck.index))
        {
            int index = playerDecks.FindIndex(x => x.index == deck.index);
            playerDecks.RemoveAt(index);
        }
        playerDecks.Add(deck);

        string json = JsonConvert.SerializeObject(playerDecks.ToArray(), Formatting.None);
        ES3.Save(userInfo.UserID + "Deck", json);

        SaveUserInfo();
    }

    public void SaveDeckList()
    {
        string json = JsonConvert.SerializeObject(playerDecks.ToArray(), Formatting.None);

        ES3.Save(userInfo.UserID + "Deck", json);

        SaveUserInfo();
    }

    public void Load_Deck()
    {
        string deckJson;
        if (ES3.KeyExists(steamID.ToString() + "Deck"))
        {
            if (ES3.Load<string>(steamID.ToString() + "Deck") != null)
            {
                deckJson = ES3.Load<string>(steamID + "Deck");
                playerDecks = JsonConvert.DeserializeObject<List<Deck>>(deckJson);

            }
        }
    }

    public void SaveUserInfo()
    {
        userInfo.UserID = steamID.ToString();
        string json = JsonUtility.ToJson(userInfo);
        
        ES3.Save(userInfo.UserID, json);
    }

    private void OnApplicationQuit()
    {
        SaveUserInfo();
    }

    public void LanguageSet()
    {
        int index = PlayerPrefs.GetInt("TribeWar_Language");
        switch (index)
        {
            case 0:
                language = Language.ENGLISH;
                break;
            case 1:
                language = Language.KOREA;
                break;
            default:
                language = Language.ENGLISH;
                break;
        }
    }

    public void ChangeCardText(int languageIndex)
    {
        switch (languageIndex)
        {
            case 0:
                language = Language.ENGLISH;
                break;
            case 1:
                language = Language.KOREA;
                break;
            default:
                language = Language.ENGLISH;
                break;
        }
        TribeStrSet();
        
        Set_CardData();
    }
}
