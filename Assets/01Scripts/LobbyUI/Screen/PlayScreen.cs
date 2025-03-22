using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;

public class PlayScreen : MonoBehaviour
{
    public Image userIcon;
    public TMP_Text username_Text;
    public TMP_Text playerRecord_Text;
    public GameObject deckListContent;

    public DeckManager deckManager;

    public TMP_Text playerText;

    [Header("버튼 텍스트 모음")]
    public TMP_Text createRoom_Text;
    public TMP_Text joinRoom_Text;
    public TMP_Text quickMatch_Text;
    public TMP_Text joinCodeInput_Text;
    public TMP_Text singlePlay_Text;
    public TMP_Text exit_Text;


    private void OnEnable()
    {
        if (!SteamManager.Initialized)
            return;

        PlayerProfileUpdate();
        MyDeckListUpdate();
    }

    private void OnDisable()
    {
        Destroy_Deck_List();
    }

    void Destroy_Deck_List()
    {
        GameObject[] allChildren = new GameObject[deckListContent.transform.childCount];

        int i = 0;

        foreach (Transform child in deckListContent.transform)
        {
            allChildren[i] = child.gameObject;
            i++;
        }

        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }

    }

    void PlayerProfileUpdate()
    {
        UserInfo user = DataManager.instance.userInfo;
        float winRate = user.WinRate();
        string winRateStr = (user.Lose == 0) ? "100.0" : winRate.ToString("F1");

        if (user.Win == 0)
            winRateStr = "0.00";

        username_Text.text = SteamFriends.GetPersonaName();
        playerRecord_Text.text = user.Win + "W " + user.Lose + "L\n( " + winRateStr + " % )";
        Texture2D steamAvatar = GetSteamImageAsTexture2D(SteamFriends.GetMediumFriendAvatar(SteamUser.GetSteamID()));

        Rect rect = new Rect(0, 0, steamAvatar.width, steamAvatar.height);

        userIcon.sprite = Sprite.Create(steamAvatar, rect, new Vector2(0.5f, 0.5f));
    }

    public Texture2D GetSteamImageAsTexture2D(int iImage)
    {
        Texture2D ret = null;
        uint ImageWidth;
        uint ImageHeight;
        bool bIsValid = SteamUtils.GetImageSize(iImage, out ImageWidth, out ImageHeight);

        if (bIsValid)
        {
            byte[] Image = new byte[ImageWidth * ImageHeight * 4];

            bIsValid = SteamUtils.GetImageRGBA(iImage, Image, (int)(ImageWidth * ImageHeight * 4));
            if (bIsValid)
            {
                ret = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, false);
                ret.LoadRawTextureData(Image);

                ret.Apply();
            }
        }
        return ret;
    }


    void MyDeckListUpdate()
    {
        deckManager.MyDeckListUpdate(deckListContent, false);
    }

    public void ChangeButtonLanguage()
    {
        LocalizationData localizationData = LocalizationManager.instance.Read("LocalizationData/UIText");

        for (int i = 0; i < localizationData.items.Count; i++)
        {
            if (localizationData.items[i].tag == "CreateRoom")
            {
                createRoom_Text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "JoinRoom")
            {
                joinRoom_Text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "QuickMatch")
            {
                quickMatch_Text.text = localizationData.items[i].value;
            }

            if (localizationData.items[i].tag == "Exit")
            {
                exit_Text.text = localizationData.items[i].value;
            }

            if (localizationData.items[i].tag == "JoinCodeInput")
            {
                joinCodeInput_Text.text = localizationData.items[i].value;
            }

            if (localizationData.items[i].tag == "SinglePlay")
            {
                singlePlay_Text.text = localizationData.items[i].value;
            }

            if (localizationData.items[i].tag == "Player")
            {
                playerText.text = localizationData.items[i].value;
            }
        }
    }
}
