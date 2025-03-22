using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkRpcFunc : NetworkBehaviour
{
    public static NetworkRpcFunc instance;

    public GameManager gameManager;
    public EntityManager entityManager;
    public CardManager cardManager;
    public MapManager mapManager;
    public EffectManager effectManager;

    private void Awake()
    {
        instance = this;
    }

    Coordinate SetCoordinate(Vector3 vectorPos, bool server)
    {
        Coordinate coordinate = new Coordinate(vectorPos);
        if (server != isServer)
        {
            coordinate.SetReverse(mapManager.mapSize);
        }

        return coordinate;
    }

    #region Entity Manager
    [ClientRpc]
    public void RpcCardMove(int entityID, Vector3 movePos, bool server)
    {
        EntityManager.instance.CardMove(entityID, SetCoordinate(movePos, server), server);
    }

    [ClientRpc]
    public void RpcAttack(int attackerID, int defenderID, bool server) => EntityManager.instance?.Attack(attackerID, defenderID, server);

    [ClientRpc]
    public void RpcOutpostAttack(int attackerID, Vector3 outpostPos, bool server)
    {
        EntityManager.instance?.OutpostAttack(attackerID, SetCoordinate(outpostPos, server), server);
    }
    //[ClientRpc]
    //public void RpcSetCoordinateData(Vector3 coordVec) => EntityManager.instance?.SetCoordinateData(coordVec);

    //[ClientRpc]
    //public void RpcSummon(bool server, string card_id, Coordinate coordinate) => EntityManager.instance?.Summon(server, card_id, coordinate);

    [ClientRpc]
    public void RpcSelectTribute(bool server, int entityID) => EntityManager.instance?.SelectMonster(server, entityID);

    [ClientRpc]
    public void RpcEffectSolve(string card_id, bool server) => effectManager.EffectSolve(card_id, server);

    [ClientRpc]
    public void RpcSelect_Effect_Target(int entityID, bool server) => effectManager.Select_Target(entityID, server);

   [ClientRpc]
    public void RpcRandomTargetEffect(int entity_id, string card_id) => EntityManager.instance?.RandomTargetEffect(entity_id, card_id);

    [ClientRpc]
    public void RpcTarget_Effect_Solver(int entityID, Vector3 tilePos, bool isServer) => EntityManager.instance?.Target_Effect_Solver(entityID, tilePos, isServer);

    #endregion

    #region Game Manager
    [ClientRpc]
    public void RpcGameResult(bool gameResult, bool server) => GameManager.instance.GameResult(gameResult, server);

    [ClientRpc]
    public void RpcLoadingComplite()
    {
        GameManager.instance?.LoadingComplited();
    }

    #endregion

    #region Card Manager

    [ClientRpc]
    public void RpcStartCardDealing() => CardManager.instance?.StartCardDealing();
    [ClientRpc]
    public void RpcReloadCard(bool server) => CardManager.instance?.ReloadCard(server);

    [ClientRpc]
    public void RpcTryPutCard(bool server, string card_id, Vector3 selectPos)
    {
        CardManager.instance?.TryPutCard(server, card_id, SetCoordinate(selectPos, server));
    }

    [ClientRpc]
    public void RpcSetMostFrontOrderInit(bool server) => CardManager.instance?.SetMostFrontOrderInit(server);

    #endregion

    #region Map Manager
    [ClientRpc]
    public void RpcSetOutpost(Vector3 outpostCoordVec, bool server)
    {
        MapManager.instance?.SetOutpost(SetCoordinate(outpostCoordVec, server), server);
    }

    #endregion

    #region Turn Manager
    [ClientRpc]
    public void RpcTurnSetup(int randomTurn) => TurnManager.instance?.TurnSetup(randomTurn);

    [ClientRpc]
    public void RpcTurnEnd() => TurnManager.instance?.TurnEnd();

    #endregion
}
