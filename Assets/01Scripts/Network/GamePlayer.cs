using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;
using Mirror;

public class GamePlayer : NetworkBehaviour
{
    [SerializeField] PlayerProfile playerProfilePrefabs;

    [Header("GamePlayer Info")]
    [SyncVar(hook = nameof(HandlePlayerNameUpdate))] public string playerName;
    [SyncVar] public int ConnectionId;
    [SyncVar] public ulong steamID_u;
    //[SyncVar(hook = nameof(HandlePlayerSetSteamID))] public ulong steamID_u;
    [SyncVar(hook = nameof(HandlePlayerWinUpdate))] public int playerWin;
    [SyncVar(hook = nameof(HandlePlayerLoseUpdate))] public int playerLose;
    [SyncVar(hook = nameof(HandlePlayerWinRateUpdate))] public float playerWinRate;
    [SyncVar(hook = nameof(HandlePlayerAvatarUpdate))] public int playeriImage;

    [SerializeField] bool isCreate = false;

    [Header("Game Info")]
    [SyncVar] public bool IsGameLeader = false;
    [SyncVar(hook = nameof(HandlePlayerLoadingStatusChange))] public bool isPlayerLoadingComplite;
    [SyncVar(hook = nameof(HandlePlayerReadyStatusChange))] public bool isPlayerReady;

    PlayerProfile profile;

    [Header("Play Info")]
    [SyncVar] public bool playerTurn;
    [SyncVar] public int turnCount;
    [SyncVar(hook = nameof(DeckCountingUpdate))] public int deckCount;

    event Action<bool> UpdateProfile;

    private MyNetworkManager game;

    private MyNetworkManager Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = MyNetworkManager.singleton as MyNetworkManager;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        CreatePlayerProfiles(); // 프로필 클라이언트쪽에서 생성이 안되는중
        UpdateProfile += ProfileUpdate;
    }
    private void Update()
    {
        
    }
    public override void OnStartClient()
    {
        Game.GamePlayers.Add(this);
    }
    public override void OnStartAuthority()
    {
        UserInfo userInfo = DataManager.instance.userInfo;
        CmdSetSteamID(SteamUser.GetSteamID().m_SteamID);
        CmdSetPlayerInfo(SteamFriends.GetPersonaName().ToString());
        CmdSetPlayerRecord(userInfo.Win, userInfo.Lose, userInfo.WinRate());
        gameObject.name = "LocalGamePlayer";
        MatchManager.instance.FindLocalGamePlayer(this);
    }

    void ProfileUpdate(bool isUpdated)
    {
        if(isUpdated)
            profile.PlayerProfileUpdate(playerWin, playerLose, playerWinRate, playerName, SteamFriends.GetLargeFriendAvatar(new CSteamID(steamID_u)), hasAuthority); // 되긴 하는데 이방법 렉 유발 가능성 있음
    }

    [Command]
    private void CmdSetSteamID(ulong steamID)
    {
        steamID_u = steamID;
    }
    [Command]
    private void CmdSetPlayerRecord(int win, int lose, float winRate)
    {
        HandlePlayerWinUpdate(playerWin, win);
        HandlePlayerLoseUpdate(playerLose, lose);
        HandlePlayerWinRateUpdate(playerWinRate, winRate);
        HandlePlayerAvatarUpdate(playeriImage, SteamFriends.GetLargeFriendAvatar(new CSteamID(steamID_u)));
    }

    public void CreatePlayerProfiles()
    {
        if (isCreate) { return; }

        GameObject playerSlot = GameObject.FindWithTag("PlayerSlot");

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            profile = Instantiate(playerProfilePrefabs);
            profile.PlayerProfileUpdate(playerWin, playerLose, playerWinRate, playerName, SteamFriends.GetLargeFriendAvatar(new CSteamID(steamID_u)), hasAuthority);
            
            if (playerSlot != null)
            {
                profile.transform.SetParent(playerSlot.transform);
                profile.transform.localScale = Vector3.one;
            }
            isCreate = true;
        }
    }
    public void HandlePlayerAvatarUpdate(int oldValue, int newValue)
    {
        if (isServer)
            playeriImage = newValue;
        UpdateProfile?.Invoke(true);
    }
    public void HandlePlayerWinUpdate(int oldValue, int newValue)
    {
        if (isServer)
            playerWin = newValue;

        UpdateProfile?.Invoke(true);
    }

    public void HandlePlayerLoseUpdate(int oldValue, int newValue)
    {
        if (isServer)
            playerLose = newValue;
        UpdateProfile?.Invoke(true);
    }

    public void HandlePlayerWinRateUpdate(float oldValue, float newValue)
    {
        if (isServer)
            playerWinRate = newValue;
        UpdateProfile?.Invoke(true);
    }

    [Command]
    private void CmdSetPlayerInfo(string playerName)
    {
        HandlePlayerNameUpdate(this.playerName, playerName);
    }
    

    public void HandlePlayerNameUpdate(string oldValue, string newValue)
    {
        if (isServer)
            playerName = newValue;
        UpdateProfile?.Invoke(true);
    }

    public void ChangeLoadingStatus()
    {
        if (hasAuthority)
        {
            CmdChangePlayerLoadingStatus();
        }
    }
    [Command]
    void CmdChangePlayerLoadingStatus()
    {
        isPlayerLoadingComplite = !isPlayerLoadingComplite;
    }
    void HandlePlayerLoadingStatusChange(bool oldValue, bool newValue)
    {
        if (isServer)
        {
            isPlayerLoadingComplite = newValue;
        }
        Game.PlayerAllLoading();
    }

    public void ChangeReadyStatus()
    {
        if (hasAuthority)
            CmdChangePlayerReadyStatus();
    }
    [Command]
    void CmdChangePlayerReadyStatus()
    {
        isPlayerReady = !isPlayerReady;
    }

    void HandlePlayerReadyStatusChange(bool oldValue, bool newValue)
    {
        if (isServer)
        {
            isPlayerReady = newValue;
        }
        profile.ReadyState(isPlayerReady);
        Game.PlayerAllReady();
    }

    public void CanEndThisGame()
    {
        if (hasAuthority)
            CanEndGame();
    }

    public void CanEndGame()
    {
        QuitLobby();

        Game.EndGame();
    }


    public void QuitLobby()
    {
        if (hasAuthority)
        {
            if (IsGameLeader)
            {
                Game.StopHost();
            }
            else
            {
                Game.StopClient();
            }
            SteamLobby.instance.LeaveLobby();
        }
    }
    private void OnDestroy()
    {
        if (hasAuthority)
        {
            SteamMatchmaking.LeaveLobby(SteamLobby.instance.currentLobby);
            game = null;
            if (SceneManager.GetActiveScene().buildIndex == 0)
                LobbyUI.instance?.StartButtonClick();
        }
        if (profile != null)
            Destroy(profile.gameObject);

        Debug.Log("LobbyPlayer destroyed. Returning to main menu.");
    }
    public override void OnStopClient()
    {
        Debug.Log(playerName + " is quiting the game.");

        if (hasAuthority == false)
            GameManager.instance?.DisconnectServerPlayer();

        Game.GamePlayers.Remove(this);
    }

    #region In Game Method


    public void DeckCountingUpdate(int oldValue, int newValue)
    {
        if (isServer)
            deckCount = newValue;
        CardManager.instance.Deck_Counting_Update(hasAuthority, newValue);
    }

    [Command]
    public void CmdSetDeckCounting(int deckCount)
    {
        DeckCountingUpdate(this.deckCount, deckCount);
    }

    [Command]
    public void CmdSetOutpostPos(Vector3 outpostCoordVec, bool server)
    {
        NetworkRpcFunc.instance.RpcSetOutpost(outpostCoordVec, server);
    }

    [Command]
    public void CmdTryPutCard(bool server, string card_id, Vector3 selectPos)
    {
        NetworkRpcFunc.instance.RpcTryPutCard(server, card_id, selectPos);
    }

    [Command]
    public void CmdSelectTribute(int entityID, bool server)
    {
        NetworkRpcFunc.instance.RpcSelectTribute(server, entityID);
    }

    [Command]
    public void CmdEffectSolve(string card_id, bool server)
    {
        NetworkRpcFunc.instance.RpcEffectSolve(card_id, server);
    }


    [Command]
    public void CmdAttack(int attackerID, int defenderID, bool server)
    {
        NetworkRpcFunc.instance.RpcAttack(attackerID, defenderID, server);
    }

    [Command]
    public void CmdOutpostAttack(int attackerID, Vector3 outpostVector, bool server)
    {
        NetworkRpcFunc.instance.RpcOutpostAttack(attackerID, outpostVector, server);
    }

    [Command]
    public void CmdReloadHandCard(bool server)
    {
        NetworkRpcFunc.instance.RpcReloadCard(server);
    }

    [Command]
    public void CmdSelectEffectTarget(int entityID, bool server)
    {
        NetworkRpcFunc.instance.RpcSelect_Effect_Target(entityID, server);
    }

    [Command]
    public void CmdRandomTargetAppoint(int entity_Id, string card_id)
    {
        NetworkRpcFunc.instance.RpcRandomTargetEffect(entity_Id, card_id);
    }

    [Command]
    public void CmdCardMove(int entityID, bool targetPlyaer, Vector3 movePos, bool server)
    {
        NetworkRpcFunc.instance.RpcCardMove(entityID, movePos, server);
    }


    [Command]
    public void CmdTurnSetup()
    {
        NetworkRpcFunc.instance.RpcStartCardDealing();
        NetworkRpcFunc.instance.RpcTurnSetup(UnityEngine.Random.Range(0, 2));
    }

    [Command]
    public void CmdTurnEnd()
    {
        NetworkRpcFunc.instance.RpcTurnEnd();
    }

    [Command]
    public void CmdGameResult(bool gameResult, bool server)
    {
        NetworkRpcFunc.instance.RpcGameResult(gameResult, server);
    }

    [Command]
    public void CmdMoveEffect(int entity_Id, Vector3 tilePos, bool server)
    {
        NetworkRpcFunc.instance.RpcTarget_Effect_Solver(entity_Id, tilePos, server);
    }


    #endregion
}
