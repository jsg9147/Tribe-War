using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using Steamworks;

public class MessageManager : NetworkBehaviour
{
    public static MessageManager instance;
    [SerializeField] private TMP_InputField messageBox;
    [SerializeField] private TMP_Text messagePrefab;
    [SerializeField] private GameObject chat_Content;
    [SyncVar(hook = nameof(HandleNewMessageText))] public string messageTextSynced = "New Text";

    private float time_Send, time_Current;
    private void Awake()
    {
        MakeInstance();
    }
    // Start is called before the first frame update
    void Start()
    {
        messageBox.onSubmit.AddListener(delegate { SendMessageToPlayers(); });
    }

    void MakeInstance()
    {
        if (instance == null)
            instance = this;
    }

    public void SendMessageToPlayers()
    {
        if (!string.IsNullOrEmpty(messageBox.text))
        {
            string newMessage = SteamFriends.GetPersonaName() + " : " + messageBox.text;
            CmdSendMessageToPlayers(newMessage);

            messageBox.ActivateInputField();
        }
    }
    public void HandleNewMessageText(string oldValue, string newValue)
    {
        if (isServer)
            messageTextSynced = newValue;
        if (isClient)
        {
            UpdateMessageText(newValue);
        }
    }
    void UpdateMessageText(string newMessage)
    {
        time_Current = Time.time - time_Send;
        if (time_Current < 0.5f)
            return;

        TMP_Text msg = Instantiate(messagePrefab);

        msg.text = newMessage;
        msg.transform.SetParent(chat_Content.transform);
        msg.transform.localScale = Vector3.one;
        messageBox.text = "";
        time_Send = Time.time;
    }
    [Command(requiresAuthority = false)]
    void CmdSendMessageToPlayers(string newMessage)
    {
        HandleNewMessageText(messageTextSynced, newMessage);
    }
}
