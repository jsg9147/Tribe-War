using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public static TextController instance;

    public TMP_Text limitTime_Text;
    public TMP_Text canSummon_Text;
    public TMP_Text canMove_Text;
    public TMP_Text RemainingCard_Text;
    public TMP_Text Grave_Text;
    public TMP_Text TurnEnd_Text;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeButtonLanguage();
    }
    public void ChangeButtonLanguage()
    {
        LocalizationData localizationData = LocalizationManager.instance.IngameText;

        for (int i = 0; i < localizationData.items.Count; i++)
        {
            if (localizationData.items[i].tag == "TimeLimit")
            {
                limitTime_Text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CanSummon")
            {
                canSummon_Text.text = localizationData.items[i].value + " : ";
            }
            if (localizationData.items[i].tag == "CanMove")
            {
                canMove_Text.text = localizationData.items[i].value + " : ";
            }

            if (localizationData.items[i].tag == "RemainingCard")
            {
                RemainingCard_Text.text = localizationData.items[i].value;
            }

            if (localizationData.items[i].tag == "Grave")
            {
                Grave_Text.text = localizationData.items[i].value;
            }

            if (localizationData.items[i].tag == "TurnEnd")
            {
                TurnEnd_Text.text = localizationData.items[i].value;
            }
        }
    }
}
