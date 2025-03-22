using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Steamworks;
using Mirror;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby instance;

    NetworkManager networkManager;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;
    protected Callback<LobbyMatchList_t> Callback_lobbyList;
    protected Callback<LobbyDataUpdate_t> Callback_lobbyInfo;

    public List<CSteamID> lobbyIDS = new List<CSteamID>();
    public CSteamID currentLobby;

    private const string HostAddressKey = "HostAdress";
    private const string GameKey = "MSGgames";
    private const string GameValue = "Tribu_War_In_MSGgames";

    public TMP_InputField joinCode_InputText;
    public TMP_Text invite_Text;
    public string inviteCode;

    bool inviteRoom;
    bool _joinCodeRoom;
    bool _createLobby;

    bool isMatch = false;

    struct LobbyMetaData
    {
        public string m_Key;
        public string m_Value;
    }

    struct LobbyMembers
    {
        public CSteamID m_SteamID;
        public LobbyMetaData[] m_Data;
    }
    struct Lobby
    {
        public CSteamID m_SteamID;
        public CSteamID m_Owner;
        public LobbyMembers[] m_Members;
        public int m_MemberLimit;
        public LobbyMetaData[] m_Data;
    }

    private void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        if (!SteamManager.Initialized) { return; }

        MakeInstance();
        CSteamID steamID = SteamUser.GetSteamID();
        //StartCoroutine(WebMain.instance.web.Login(steamID.m_SteamID.ToString()));

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        Callback_lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbiesList);
        Callback_lobbyInfo = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyInfo);
    }

    void CreateInviteCode()
    {
        inviteCode = "";
        for (int i = 0; i < 8; i++)
        {
            bool randomInt = (Random.value > 0.5f);
            if (randomInt)
                inviteCode = inviteCode + (char)Random.Range(65, 90);
            else
                inviteCode = inviteCode + (char)Random.Range(48, 57);
        }

        if (inviteRoom)
        {
            try
            {
                TMP_Text inviteCode_Text = GameObject.Find("InviteCode_Text").GetComponent<TMP_Text>();
                inviteCode_Text.text = inviteCode;
            }
            catch (System.NullReferenceException ex)
            {
                Debug.LogError(ex);
            }
        }
    }

    void MakeInstance()
    {
        if (instance == null)
            instance = this;
    }


    public void HostLobby()
    {
        inviteRoom = false;
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 2);
    }

    public void JoinLobby(CSteamID lobbyId)
    {
        SteamMatchmaking.JoinLobby(lobbyId);
    }

    public void LeaveLobby()
    {
        Debug.Log("Leave lobby");
        _createLobby = false;

        if (currentLobby != null)
            SteamMatchmaking.LeaveLobby(currentLobby);

        currentLobby = new CSteamID();
    }

    public void GetListOfLobbies(bool joinCodeRoom)
    {
        print("Get List of Lobbies");
        if (lobbyIDS.Count > 0)
            lobbyIDS.Clear();

        _joinCodeRoom = joinCodeRoom;

        SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(1);
        SteamMatchmaking.RequestLobbyList();
    }


    public void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }
        if (networkManager.isNetworkActive == false)
        {
            networkManager.StartHost();
        }

        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            HostAddressKey,
            SteamUser.GetSteamID().ToString());


        if (inviteRoom)
        {
            SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            inviteCode,
            GameValue);

            SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            GameKey,
            inviteCode);
        }
        else
        {
            SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            GameKey,
            GameValue);
        }

        _createLobby = true;
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        Debug.Log("On Lobby Enterd!");
        isMatch = true;
        currentLobby = new CSteamID(callback.m_ulSteamIDLobby);
        if (NetworkServer.active) { return; }

        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            HostAddressKey);

        networkManager.networkAddress = hostAddress;

        networkManager.StartClient();
        lobbyIDS.Clear();
    }

    void OnGetLobbiesList(LobbyMatchList_t result)
    {
        StartCoroutine(GetLobbiesList(result));
    }

    IEnumerator GetLobbiesList(LobbyMatchList_t result)
    {
        if (isMatch == false)
        {
            string value;

            value = _joinCodeRoom ? joinCode_InputText.text : GameKey;

            if (_joinCodeRoom)
                invite_Text.text = joinCode_InputText.text;

            for (int i = 0; i < result.m_nLobbiesMatching; i++)
            {
                CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
                if (SteamMatchmaking.GetLobbyData((CSteamID)lobbyID.m_SteamID, value) == GameValue)
                {
                    if (SteamMatchmaking.RequestLobbyData(lobbyID))
                    {
                        lobbyIDS.Add(lobbyID);
                    }
                }
            }

            if (!_joinCodeRoom && lobbyIDS.Count == 0)
            {
                MatchManager.instance.CreateNewLobby();
            }
        }
        else
        {
            yield return new WaitForSeconds(2f);
            isMatch = false;
            LeaveLobby();
            networkManager.StopHost();
            networkManager.StopClient();

            // 문제 있을꺼 같아서 잠시 삭제

            OnGetLobbiesList(result);
        }
    }

    void OnGetLobbyInfo(LobbyDataUpdate_t result)
    {;
        if (_createLobby || lobbyIDS.Count == 0)
        {
            return;
        }

        GameObject roomScreen = GameObject.Find("RoomScreen");

        if (roomScreen != null)
        {
            try
            {
                if (roomScreen.activeSelf)
                {
                    TMP_Text inviteCode_Text = GameObject.Find("InviteCode_Text").GetComponent<TMP_Text>();
                    inviteCode_Text.text = joinCode_InputText.text;
                }

            }
            catch (System.NullReferenceException ex)
            {
                Debug.LogError(ex);
            }
        }
        
        MatchManager.instance.AutoJoinLobby(lobbyIDS, result, _joinCodeRoom);
    }

    public void CreateNewLobby(ELobbyType lobbyType, bool isInvite)
    {
        inviteRoom = isInvite;
        CreateInviteCode();
        SteamMatchmaking.CreateLobby(lobbyType, networkManager.maxConnections);
    }


    public void StopMatching()
    {
        isMatch = false;
        LeaveLobby();
        networkManager.StopHost();
        networkManager.StopClient();
        LobbyUI.instance.StartButtonClick();
    }
}
