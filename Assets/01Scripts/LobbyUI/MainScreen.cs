using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using TMPro;
using DarkTonic.MasterAudio;

public class MainScreen : MonoBehaviour
{
    public TMP_Text welcome_Text;
    public TMP_Text NoExistsDeck_Text;

    private void Start()
    {
        if (!SteamManager.Initialized) { return; }
        WelcomeConnectText();

        NoExistsDeckTextChange();
    }

    void BtnAddListenner()
    {
        
    }

    void WelcomeConnectText()
    {
        string user_name = SteamFriends.GetPersonaName();
        welcome_Text.text = user_name + "님 \n 접속을 환영합니다.";
    }

    void NoExistsDeckTextChange()
    {
        LocalizationData localizationData = LocalizationManager.instance.Read("LocalizationData/UIText");

        for (int i = 0; i < localizationData.items.Count; i++)
        {
            if (localizationData.items[i].tag == "NoExsitDeck")
            {
                NoExistsDeck_Text.text = localizationData.items[i].value;
            }
        }
    }
}
