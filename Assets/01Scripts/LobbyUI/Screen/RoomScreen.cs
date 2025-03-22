using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomScreen : MonoBehaviour
{
    [SerializeField] private GameObject deckListContent;
    [SerializeField] private GameObject ChatContent;
    [SerializeField] private TMPro.TMP_Text readyButtonText;

    public GameObject playerSlot;
    public PlayerProfile profile;

    public TMP_Text player_Text;
    public TMP_Text chatInput_Text;
    public TMP_Text send_Text;
    public TMP_Text inviteCode_Text;
    public TMP_Text ready_Text;
    public TMP_Text exit_Text;

    public DeckManager deckManager;
    public MyNetworkManager networkManager;

    private void OnEnable()
    {
        MyDeckListUpdate();
        MessageReset();
        ChangeButtonLanguage();
    }

    void MessageReset()
    {
        Transform[] childrenList = ChatContent.GetComponentsInChildren<Transform>();

        if (childrenList != null)
        {
            for (int i = 1; i < childrenList.Length; i++)
            {
                if (childrenList[i] != transform)
                    Destroy(childrenList[i].gameObject);
            }
        }
    }

    void MyDeckListUpdate()
    {
        foreach (Transform child in deckListContent.transform)
        {
            Destroy(child.gameObject);
        }
        deckManager.MyDeckListUpdate(deckListContent, false);
    }
    public void ChangeButtonLanguage()
    {
        LocalizationData localizationData = LocalizationManager.instance.UIText;
        for (int i = 0; i < localizationData.items.Count; i++)
        {
            if (localizationData.items[i].tag == "Player")
            {
                player_Text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "ChatInput")
            {
                chatInput_Text.text = localizationData.items[i].value;
            }
            if (localizationData.items[i].tag == "Send")
            {
                send_Text.text = localizationData.items[i].value;
            }

            if (localizationData.items[i].tag == "Exit")
            {
                exit_Text.text = localizationData.items[i].value;
            }

            if (localizationData.items[i].tag == "InviteCode")
            {
                inviteCode_Text.text = localizationData.items[i].value;
            }

            if (localizationData.items[i].tag == "Ready")
            {
                ready_Text.text = localizationData.items[i].value;
            }
        }
    }
}
