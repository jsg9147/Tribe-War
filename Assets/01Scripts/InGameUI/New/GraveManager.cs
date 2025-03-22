using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraveManager : MonoBehaviour
{
    public GameObject graveCard_Prefab;

    public GameObject playerGraveBackground;
    public GameObject opponentGraveBackground;

    public CardUI playerGraveCardUI;
    public CardUI opponentGraveCardUI;

    public TMP_Text playerGraveCountText;
    public TMP_Text oppnentGraveCountText;


    public List<Card> playerGraveCards = new List<Card>();
    public List<Card> oppenentGraveCards = new List<Card>();

    public GameObject playerGraveContent;
    public GameObject opponentGraveContent;

    public List<CardUI> playerList;
    public List<CardUI> enermyList;


    public void Grave_Card_List_On(GameObject graveContents)
    {
        graveContents.SetActive(!graveContents.activeSelf);
    }

    public void AddGraveCard(Card card, bool isMine)
    {
        GameObject graveCard = Instantiate(graveCard_Prefab);
        graveCard.GetComponent<CardUI>().Setup(card);

        graveCard.name = card.name;
        if (isMine)
        {
            if (playerGraveBackground.activeSelf)
            {
                playerGraveBackground.SetActive(false);
            }
            playerGraveCardUI.Setup(card);
            playerGraveCards.Add(card);
            playerGraveCountText.text = playerGraveCards.Count.ToString();
            graveCard.transform.SetParent(playerGraveContent.transform, false);
        }
        else
        {
            if (opponentGraveBackground.activeSelf)
            {
                opponentGraveBackground.SetActive(false);
            }

            oppenentGraveCards.Add(card);
            opponentGraveCardUI.Setup(card);
            oppnentGraveCountText.text = oppenentGraveCards.Count.ToString();
            graveCard.transform.SetParent(opponentGraveContent.transform, false);
        }
    }
}
