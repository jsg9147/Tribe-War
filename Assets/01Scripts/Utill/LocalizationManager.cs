using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //인스펙터 테이블에 노출시키기 위함.
public class LocalizationData
{
    public List<LocalizationItem> items;
}

[System.Serializable]
public class LocalizationItem
{
    public string key;
    public int index;
    public string value;
    public string tag;
}

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

    const string ENGLISH = "English";
    const string KOREA = "Korea";

    public string language;
    public LocalizationData UIText;
    public LocalizationData IngameText;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Init();
    }
    void Init()
    {
        UIText = Read("LocalizationData/UIText");
        IngameText = Read("LocalizationData/InGameText");
        //그 옵션 저장하기 좋았던 그거 쓰면 될듯
    }

    public void ChangeTextLanguage()
    {
        UIText = Read("LocalizationData/UIText");
        IngameText = Read("LocalizationData/InGameText");
    }

    public void SetLanguage(string _language) => this.language = _language;

    public LocalizationData Read(string fileName)
    {
        LanguageSet();
        List<Dictionary<string, object>> data_Dialog = CSVReader.Read(fileName);
        LocalizationData localizationData = new LocalizationData();
        localizationData.items = new List<LocalizationItem>();
        for (int i = 0; i < data_Dialog.Count; i++)
        {
            if (data_Dialog[i]["Language"].ToString() == language)
            {
                LocalizationItem item = new LocalizationItem();
                item.index = int.Parse(data_Dialog[i]["Index"].ToString());
                if (!string.IsNullOrEmpty(data_Dialog[i]["Content"].ToString()))
                {
                    item.value = data_Dialog[i]["Content"].ToString();
                }

                if (!string.IsNullOrEmpty(data_Dialog[i]["Tag"].ToString()))
                {
                    item.tag = data_Dialog[i]["Tag"].ToString();
                }
                localizationData.items.Add(item);
            }

        }

        return localizationData;
    }

    public void LanguageSet()
    {
        int index = PlayerPrefs.GetInt("TribeWar_Language");
        switch (index)
        {
            case 0:
                language = ENGLISH;
                break;
            case 1:
                language = KOREA;
                break;
            default:
                language = ENGLISH;
                break;
        }
        
    }

    public string GetIngameText(string tag)
    {
        string text = "";
        for (int i = 0; i < IngameText.items.Count; i++)
        {
            if (IngameText.items[i].tag == tag)
            {
                text = IngameText.items[i].value;
            }
        }
        return text;
    }
}
