using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    /// <summary>
    /// The unique identifier of this player.
    /// </summary>
    public int id;

    /// <summary>
    /// The unique connection identifier of this player.
    /// </summary>
    public int connectionId;

    /// <summary>
    /// The unique network instance identifier of this player.
    /// </summary>
    public UserInfo netId;

    /// <summary>
    /// The nickname of this player.
    /// </summary>
    public string nickname;

    /// <summary>
    /// True if this player is currently connected to the server; false otherwise.
    /// </summary>
    public bool isConnected;

    public int numTurn;
}
