using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOption : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject optionWindow;

    [Header("텍스트 모음")]
    [SerializeField] TMP_Text resolution_text;
    [SerializeField] TMP_Text language_text;
    [SerializeField] TMP_Text sound_text;

    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown languageDropdown;
    [SerializeField] Toggle fullscreenBtn;

    List<Resolution> resolutions = new List<Resolution>();

    [SerializeField] Slider soundSlider;
    [SerializeField] TMP_Text soundValue_Text;
    int resolutionNum;
    int languageIndex;
    float masterVolume;

    private void Start()
    {
        InitUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionWindow.SetActive(false);
        }
    }

    public void InitUI()
    {        
        languageIndex = PlayerPrefs.GetInt("TribeWar_Language", 0);
        LocalizationManager.instance.LanguageSet();

        Screen.SetResolution(PlayerPrefs.GetInt("TribeWar_Width", 1600), PlayerPrefs.GetInt("TribeWar_Height", 900), false);

        AdjustVolume(PlayerPrefs.GetFloat("TribeWar_Sound", 1f));

        soundSlider.value = PlayerPrefs.GetFloat("TribeWar_Sound", 1f);

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width * 9 == Screen.resolutions[i].height * 16)
            {
                if (resolutions.Exists(x => x.width == Screen.resolutions[i].width))
                {

                }
                else
                {
                    resolutions.Add(Screen.resolutions[i]);
                }
            }
        }
        resolutionDropdown.options.Clear();
        resolutions.Reverse();

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.ToString().Split('@')[0];
            resolutionDropdown.options.Add(option);

            if (item.width == PlayerPrefs.GetInt("TribeWar_Width"))
            {
                resolutionDropdown.value = optionNum;
            }

            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();

        //fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;

        languageDropdown.value = languageIndex;

        Text_Language_Refresh();

    }



    void Text_Language_Refresh()
    {
        LocalizationData localizationData = LocalizationManager.instance.Read("LocalizationData/Option");

        for (int i = 0; i < localizationData.items.Count; i++)
        {
            if (localizationData.items[i].index == 0)
            {
                resolution_text.text = localizationData.items[i].value.ToString();
            }
            if (localizationData.items[i].index == 1)
            {
                language_text.text = localizationData.items[i].value.ToString();
            }
            if (localizationData.items[i].index == 2)
            {
                sound_text.text = localizationData.items[i].value.ToString();
            }
        }
    }
    public void Resolation_DropboxOptionChange(TMP_Dropdown x)
    {
        resolutionNum = x.value;
    }
    public void LanguageOption_Change(TMP_Dropdown x)
    {
        languageIndex = x.value;
    }

    //public void FullScreenBtn(bool isFull)
    //{
    //    screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    //}

    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
        SoundTextValue(newVolume * 100);
    }

    public void SoundTextValue(float value)
    {
        int soundValue = (int)value;
        soundValue_Text.text = soundValue.ToString() + " %";
    }

    public void OkBtnClick()
    {
        try
        {
            Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, FullScreenMode.Windowed);
        }
        catch(System.ArgumentException ex)
        {
            Debug.Log(ex);
        }

        LocalizationManager.instance.LanguageSet();

        PlayerPrefs.SetInt("TribeWar_Language", languageIndex);
        
        Text_Language_Refresh();
        LocalizationManager.instance.ChangeTextLanguage();
        PlayerPrefs.SetFloat("TribeWar_Sound", AudioListener.volume);

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LobbyUI.instance.LanguageChange_Button();
            LobbyUI.instance.OptionButton();
            DataManager.instance.ChangeCardText(languageIndex);
        }
        else
        {
            DataManager.instance.ChangeCardText(languageIndex);
            TextController.instance.ChangeButtonLanguage();
        }
        menuPanel.SetActive(false);
        optionWindow.SetActive(false);

        if (GameManager.instance != null)
        {
            GameManager.instance.clickBlock = false;
        }
    }
}
