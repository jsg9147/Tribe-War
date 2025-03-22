using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI instance;

    public TMPro.TMP_Text version;

    public DeckManager deckManager;

    public GameObject mainScreen;
    public GameObject playScreen;
    public GameObject myDeckScreen;
    public GameObject deckEditor;
    public GameObject shopScreen;
    public RoomScreen roomScreen;
    public GameObject optionPanel;
    public GameObject noExistDeckWindow;

    public GameObject matchLoadingWindow;

    public GameObject deckNoticePanel;

    public Transform cardInfo_Main_Layout;
    [Header("도움말 UI")]
    public GameObject helpPanel;
    public GameObject help_Screen;
    public GameObject deckEditHelp_Screen;
    public GameObject dualScreenHelp_Screen;
    public GameObject cardInfoHelp_Screen;

    [Header("버튼 이미지")]
    public List<Image> lobbyBtns;

    [Header("영어 버튼 이미지")]
    public List<Sprite> englishBtns;

    [Header("한글 버튼 이미지")]
    public List<Sprite> koreaBtns;

    public TMP_Text joinCode_Text;

    private void Start()
    {
        version.text = Application.version;
        MakeInstance();
        MainScreenOn();
       
    }

    #region gameUI

    void MakeInstance()
    {
        instance = this;
    }

    public void ClickSound() => MasterAudio.PlaySound("ButtonClick");

    void ScreenClear()
    {
        mainScreen.SetActive(false);
        playScreen.SetActive(false);
        deckEditor.SetActive(false);
        myDeckScreen.SetActive(false);
        shopScreen.SetActive(false);
        roomScreen.gameObject.SetActive(false);
        noExistDeckWindow.SetActive(false);
    }

    public void MainScreenOn()
    {
        ScreenClear();
        mainScreen.SetActive(true);
        LanguageChange_Button();
    }

    public void StartButtonClick()
    {
        ClickSound();
        ScreenClear();
        if (DataManager.instance.playerDecks.Count > 0)
        {
            playScreen.SetActive(true);
        }
        else
        {
            noExistDeckWindow.SetActive(true);
        }
    }

    public void QuickMatchButtonClick(bool isActive)
    {
        MasterAudio.PlaySound("MatchSound");
        matchLoadingWindow.SetActive(isActive);
    }

    public void DeckEditorButtonClick()
    {
        ClickSound();
        ScreenClear();
        myDeckScreen.SetActive(true);
    }

    public void DeckNoticeWindow()
    {
        ClickSound();
        deckNoticePanel.SetActive(!deckNoticePanel.activeSelf);
    }

    public void SelectEditMyDeck(Deck deck)
    {
        ScreenClear();
        deckEditor.SetActive(true);
        deckManager.Edit_Deck_Setup(deck);
        deckManager.ChangeLanguage();
    }

    public void ShopButtonClick()
    {
        ClickSound();
        ScreenClear();
        shopScreen.SetActive(true);
    }

    public void CreateRoomButtonClick()
    {
        ClickSound();
        ScreenClear();
        roomScreen.gameObject.SetActive(true);
        joinCode_Text.text = "";
    }

    public void OptionButton()
    {
        ClickSound();
        optionPanel.SetActive(!optionPanel.activeSelf);
    }

    public void TutorialButton()
    {
        ClickSound();
        SceneManager.LoadScene("Tutorial");
    }
    public void SinglePlay()
    {
        ClickSound();
        SceneManager.LoadScene("SinglePlay");
    }

    public void ExitButton()
    {
        ClickSound();
        Application.Quit();
    }
    #endregion

    public void LanguageChange_Button()
    {
        List<Sprite> btnList;
        int languageIndex = PlayerPrefs.GetInt("TribeWar_Language");
        if (languageIndex == 0)
        {
            btnList = englishBtns;
        }
        else if (languageIndex == 1)
        {
            btnList = koreaBtns;
        }
        else
        {
            btnList = new List<Sprite>();
        }

        for (int i = 0; i < lobbyBtns.Count; i++)
        {
            try
            {
                lobbyBtns[i].sprite = btnList[i];
            }
            catch(System.IndexOutOfRangeException exception)
            {
                Debug.Log(exception.Message + "\nLanguage Setting is wrong");
            }
        }
        playScreen.GetComponent<PlayScreen>().ChangeButtonLanguage();
        roomScreen.ChangeButtonLanguage();
        helpPanel.GetComponent<HelpScreen>().ChangeLanguage();
        deckManager.ChangeLanguage();
    }


    #region HelpUI
    void HelpScreen_Claer()
    {
        deckEditHelp_Screen.SetActive(false);
        dualScreenHelp_Screen.SetActive(false);
        cardInfoHelp_Screen.SetActive(false);
        help_Screen.SetActive(false);
        cardInfo_Main_Layout.DOLocalMoveX(0, 0f);
    }

    public void HelpScreen_SetActive(bool isActive)
    {
        ClickSound();
        ScreenClear();
        HelpScreen_Claer();
        helpPanel.SetActive(true);
        help_Screen.SetActive(isActive);

        if (isActive == false)
        {
            helpPanel.SetActive(false);
            MainScreenOn();
        }
    }

    public void Help_Deck_Edit_SetActive(bool isActive)
    {
        ClickSound();
        HelpScreen_Claer();
        deckEditHelp_Screen.SetActive(true);
    }

    public void Help_Dual_Screen_SetActive(bool isActive)
    {
        ClickSound();
        HelpScreen_Claer();
        dualScreenHelp_Screen.SetActive(true);
    }

    public void Help_Card_Info_SetActive(bool isActive)
    {
        ClickSound();
        HelpScreen_Claer();
        cardInfoHelp_Screen.SetActive(true);
    }

    public void CardInfo_Next_Page(bool isNext)
    {
        ClickSound();
        if (isNext)
            cardInfo_Main_Layout.DOLocalMoveX(-1920, 0.3f);
        else
            cardInfo_Main_Layout.DOLocalMoveX(0, 0.3f);
    }

    #endregion

    public void NoExsitDeck_OKClick()
    {
        DeckManager.instance.AddDeckButtonClick();
    }
}
