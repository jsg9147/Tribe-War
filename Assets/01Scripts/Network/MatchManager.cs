using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class MatchManager : MonoBehaviour
{
    public static MatchManager instance;

    public string lobbyName;
    public GameObject localGamePlayerObject;
    public GamePlayer localGamePlayerScript;

    private void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (instance == null)
            instance = this;
    }
    public void FindLocalGamePlayer(GamePlayer localPlayer)
    {
        localGamePlayerObject = localPlayer.gameObject;
        localGamePlayerScript = localPlayer;
    }

    public void CreateInviteLobby()
    {
        Debug.Log("Create invite lobby");
        ELobbyType newLobbyType = ELobbyType.k_ELobbyTypePublic;
        SteamLobby.instance.CreateNewLobby(newLobbyType, true); ;
    }

    public void CreateNewLobby()
    {
        Debug.Log("Create new lobby");
        ELobbyType newLobbyType = ELobbyType.k_ELobbyTypePublic;
        SteamLobby.instance.CreateNewLobby(newLobbyType, false);
    }

    public void MatchingStart()
    {
        SteamLobby.instance.StopMatching();
        SteamLobby.instance.GetListOfLobbies(false);
        Debug.Log("Looking for opponent ...");
    }

    public void JoinCodeRoom()
    {
        SteamLobby.instance.GetListOfLobbies(true);
    }

    public void AutoJoinLobby(List<CSteamID> lobbyIDS, LobbyDataUpdate_t result, bool joinRoom)
    {
        for (int i = 0; i < lobbyIDS.Count; i++)
        {
            if (lobbyIDS[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                SteamLobby.instance.JoinLobby(lobbyIDS[i]);

                if (joinRoom)
                    LobbyUI.instance.CreateRoomButtonClick();

                break;
            }
        }
    }



    public void MatchingCancel()
    {
        SteamLobby.instance.StopMatching();
    }
}
