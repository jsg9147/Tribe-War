using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpScreen : MonoBehaviour
{
    public TMP_Text help_Titlebar;

    public TMP_Text editInfo_Title_text;
    public TMP_Text ingameInfo_text;
    public TMP_Text cardInfo_text;

    public TMP_Text editInfo_Text_1;
    public TMP_Text editInfo_Text_2;

    public List<TMP_Text> ingameExplanation_List;

    public TMP_Text noExistDeck_Text;

    public TMP_Text cardInfo_move_Text;
    public TMP_Text cardInfo_name_Text;
    public TMP_Text cardInfo_level_Text;
    public TMP_Text cardInfo_attackType_Text;
    public TMP_Text cardInfo_effect_Text;

    public TMP_Text move_straight_Text1;
    public TMP_Text move_diagonal_Text2;
    public TMP_Text move_8Direction_Text3;

    public void ChangeLanguage()
    {
        LocalizationData localizationData = LocalizationManager.instance.UIText;

        for (int i = 0; i < localizationData.items.Count; i++)
        {
            if (localizationData.items[i].tag == "Help")
            {
                help_Titlebar.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "EditInfoTitle")
            {
                editInfo_Title_text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "IngameExplanationTitle")
            {
                ingameInfo_text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CardInfoTitle")
            {
                cardInfo_text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CardInfoText1")
            {
                editInfo_Text_1.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CardInfoText2")
            {
                editInfo_Text_2.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "IngameExplanation1")
            {
                ingameExplanation_List[0].text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "IngameExplanation2")
            {
                ingameExplanation_List[1].text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "IngameExplanation3")
            {
                ingameExplanation_List[2].text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "IngameExplanation4")
            {
                ingameExplanation_List[3].text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "IngameExplanation5")
            {
                ingameExplanation_List[4].text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "IngameExplanation6")
            {
                ingameExplanation_List[5].text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "IngameExplanation7")
            {
                ingameExplanation_List[6].text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "NoExistDeck")
            {
                noExistDeck_Text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CardInfoMove")
            {
                cardInfo_move_Text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CardInfoName")
            {
                cardInfo_name_Text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CardInfoLevel")
            {
                cardInfo_level_Text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CardInfoAttackType")
            {
                cardInfo_attackType_Text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "CardInfoEffect")
            {
                cardInfo_effect_Text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "MoveStraight")
            {
                move_straight_Text1.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "MoveDiagonal")
            {
                move_diagonal_Text2.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "Move8Direction")
            {
                move_8Direction_Text3.text = localizationData.items[i].value;
            }
        }
    }
}
