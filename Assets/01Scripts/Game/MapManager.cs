using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MapManager : MonoBehaviour
{
    public static MapManager instance
    {
        get; private set;
    }

    void Awake() => instance = this;

    public int Life = 100;

    public PRS originPRS;

    public GameObject TilePrefab;
    public GameObject ParentTile;
    public int mapSize;
    public float tilePrefabSizeX = 2.5f;
    public float tilePrefabSizeY = 2.5f;

    public int moveMap_x, moveMap_y;

    public Tile[,] mapData;

    List<List<Tile>> map = new List<List<Tile>>();

    public int outpostMaxCount;
    int outpostCount;
    public bool selectOutpostComplete;

    public List<Outpost> player_OutpostList = new List<Outpost>();
    public List<Outpost> opponent_OutpostList = new List<Outpost>();
    public int livePlayerOutpost, liveOpponentOutpost;

    public bool select_tile_mode;

    Entity temp_entity;

    public List<Coordinate> canSummonPos, enermySummonPos;


    private void Start()
    {
        Setup();
        generateMap();
        ChangeColorOutOfTile(Color.red);
    }

    private void Setup()
    {
        mapData = new Tile[mapSize, mapSize];
        selectOutpostComplete = false;
        outpostCount = 0;

        canSummonPos = new List<Coordinate>();
        enermySummonPos = new List<Coordinate>();
    }

    public Tile GetTile(Coordinate coordinate) => mapData[coordinate.x, coordinate.y];

    #region 튜토리얼에서 사용중
    public void Tile_ClickBlock(bool isBlock)
    {
        foreach (Tile tile in mapData)
        {
            tile.clickBlock = isBlock;
        }
    }
    public void Tile_Color_Reset()
    {
        foreach (Tile tile in mapData)
        {
            tile.ResetColor();
        }
    }

    public void Can_Summon_Tile_Display(bool isDisplay)
    {
        foreach (Coordinate coord in canSummonPos)
        {
            if (isDisplay)
            {
                mapData[coord.x, coord.y].ColorChange_Rock(true, Color.green);

            }
            else
            {
                mapData[coord.x, coord.y].ColorChange_Rock(false, Color.white);

            }
            mapData[coord.x, coord.y].clickBlock = true;
        }
    }

    #endregion
    void generateMap()
    {
        map = new List<List<Tile>>();
        for (int i = 0; i < mapSize; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++)
            {
                Tile tile = ((GameObject)Instantiate(TilePrefab, new Vector3((i - Mathf.Floor(mapSize / 2)) * tilePrefabSizeX + moveMap_x, (j - Mathf.Floor(mapSize / 2)) * tilePrefabSizeY + moveMap_y, -30), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                tile.name = "(" + i.ToString() + ", " + j.ToString() + ")";
                tile.transform.parent = ParentTile.transform;

                tile.coordinate = new Coordinate(i, j);

                MapColorChange(i, j, tile);
                mapData[i, j] = tile;
                tile.canSelect = true;
                row.Add(tile);
            }
            map.Add(row);
        }

        if (GameManager.instance.MultiMode)
        {
            GameManager.instance.localGamePlayerScript.ChangeLoadingStatus();
        }
    }

    void MapColorChange(int x, int y, Tile tile)
    {
        Color color;
        int calX, calY;
        calX = x % 2;
        calY = y % 2;

        ColorUtility.TryParseHtmlString("#EFDE33", out color);

        if (calX == calY)
        {
            tile.SetOriginColor(color);
            tile.ResetColor();
        }
        else
        {
            tile.SetOriginColor(Color.white);
            tile.ResetColor();
        }

    }

    void ChangeColorOutOfTile(Color color)
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = mapSize; j > (mapSize / 2); j--)
            {
                mapData[i, j - 1].ChangeTileColor(color);

                if (color == Color.red)
                    mapData[i, j - 1].canSelect = false;

                if (color == Color.white)
                    mapData[i, j - 1].canSelect = true;

            }
        }
    }
    public Tile coordinateTile(Coordinate coordinate)
    {
        return mapData[coordinate.x, coordinate.y];
    }

    public void SetupOutpost(Tile tile)
    {
        if (selectOutpostComplete)
            return;

        if (tile.coordinate.y >= mapSize / 2)
            return;

        if (tile.tileState != TileState.empty)
            return;

        Debug.Log("Seleted outpost : " + tile.coordinate.vector3Pos);

        outpostCount++;

        tile.SetupSelectOutpost(true);

        if (GameManager.instance.MultiMode)
        {
            GameManager.instance.localGamePlayerScript.CmdSetOutpostPos(tile.coordinate.vector3Pos, NetworkRpcFunc.instance.isServer);
        }
        else
        {
            SetOutpost(tile.coordinate, true);
        }

        if (outpostCount < outpostMaxCount)
        {
            selectOutpostComplete = false;
        }
        else
        {
            selectOutpostComplete = true;
            foreach (var tileSet in mapData)
            {
                tileSet.canSelect = true;
                tileSet.ResetColor();
            }
        }
    }

    public void MapTileInit()
    {
        foreach (var tile in mapData)
        {
            tile.ColorChange_Rock(false, Color.white);
            tile.clickBlock = false;
        }
    }

    public void SelectMode(Entity entity, Ability ability)
    {
        select_tile_mode = true;
        GameManager.instance.Notification(LocalizationManager.instance.GetIngameText("Move"));
        int value = 0;

        foreach (Effect effect in ability.effects)
        {
            if (effect.effectClass == EffectClass.move)
            {
                value = effect.value;
            }
        }

        foreach (Tile tile in mapData)
        {
            if (tile.tileState == TileState.empty)
            {
                if (tile.coordinate.MaxDistance(entity.coordinate, value))
                {
                    tile.ColorChange_Rock(true, Color.green);
                }
                else
                {
                    tile.clickBlock = true;
                }
            }
        }
        temp_entity = entity;
    }
    public void Select_Effect_Tile(Tile targetTile)
    {
        if (select_tile_mode)
        {
            if (targetTile.tileState == TileState.empty)
            {
                if (GameManager.instance.MultiMode)
                {
                    GameManager.instance.localGamePlayerScript.CmdMoveEffect(temp_entity.id, targetTile.coordinate.vector3Pos, NetworkRpcFunc.instance.isServer);
                }
                else
                {
                    EntityManager.instance.Target_Effect_Solver(temp_entity.id, targetTile.coordinate.vector3Pos, true); ;
                }
                select_tile_mode = false;
            }
        }
    }

    public void SetOutpost(Coordinate coordinate, bool server)
    {
        bool isMine = GameManager.instance.IsMine(server);

        Outpost outpost;

        outpost = coordinateTile(coordinate).outpost;
        coordinateTile(coordinate).OutpostSetActive(Life, isMine);

        if (isMine)
        {
            player_OutpostList.Add(outpost);
        }
        else
        {
            opponent_OutpostList.Add(outpost);
        }

        SetCanSpawnPoint(outpost, isMine);

        if ((player_OutpostList.Count + opponent_OutpostList.Count) == outpostMaxCount * 2)
        {
            foreach (var opponent_Outpost in opponent_OutpostList)
            {
                coordinateTile(opponent_Outpost.coordinate).outpost_object.SetActive(true);
                coordinateTile(opponent_Outpost.coordinate).OutpostSetActive(Life, false);
                coordinateTile(opponent_Outpost.coordinate).Outpost_Life_Setup();
            }
            livePlayerOutpost = player_OutpostList.Count;
            liveOpponentOutpost = opponent_OutpostList.Count;

            if (GameManager.instance.MultiMode)
            {
                if (NetworkRpcFunc.instance.isServer)
                    GameManager.instance.StartGame();
            }
            else
            {
                GameManager.instance.StartGame();
            }
        }
    }

    void SetCanSpawnPoint(Outpost outpost, bool isMine)
    {
        int minX = outpost.coordinate.x - 1;
        int minY = outpost.coordinate.y - 1;
        int maxX = outpost.coordinate.x + 1;
        int maxY = outpost.coordinate.y + 1;

        if (minX <= 0)
        {
            minX = 0;
        }
        if (minY <= 0)
        {
            minY = 0;
        }
        if (maxX >= mapSize)
        {
            maxX = mapSize - 1;
        }
        if (maxY >= mapSize)
        {
            maxY = mapSize - 1;
        }

        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                if (outpost.isDie == false)
                {
                    if (isMine)
                    {
                        if (mapData[i, j].canSpawn == CanSpawn.opponentCanSpawn)
                        {
                            mapData[i, j].canSpawn = CanSpawn.all;
                        }
                        else
                        {
                            mapData[i, j].canSpawn = CanSpawn.playerCanSpawn;
                        }

                        canSummonPos.Add(new Coordinate(i, j));
                    }
                    else
                    {
                        if (mapData[i, j].canSpawn == CanSpawn.playerCanSpawn)
                        {
                            mapData[i, j].canSpawn = CanSpawn.all;
                        }
                        else
                        {
                            mapData[i, j].canSpawn = CanSpawn.opponentCanSpawn;
                        }
                        enermySummonPos.Add(new Coordinate(i, j));
                    }
                }
                else
                {
                    if (isMine)
                    {
                        if (mapData[i, j].canSpawn == CanSpawn.all)
                        {
                            mapData[i, j].canSpawn = CanSpawn.opponentCanSpawn;
                        }
                        else
                        {
                            mapData[i, j].canSpawn = CanSpawn.nothing;
                        }

                        canSummonPos.Remove(canSummonPos.Find(x => x.vector3Pos == new Coordinate(i,j).vector3Pos));
                    }
                    else
                    {
                        if (mapData[i, j].canSpawn == CanSpawn.all)
                        {
                            mapData[i, j].canSpawn = CanSpawn.playerCanSpawn;
                        }
                        else
                        {
                            mapData[i, j].canSpawn = CanSpawn.nothing;
                        }
                        enermySummonPos.Remove(canSummonPos.Find(x => x.vector3Pos == new Coordinate(i, j).vector3Pos));
                    }
                }
            }
        }
    }

    public void OutpostDestroy(Outpost outpost, bool isMine)
    {
        if (outpost.belong == EntityBelong.Player)
            livePlayerOutpost--;
        else if(outpost.belong == EntityBelong.Enermy)
            liveOpponentOutpost--;

        int minX = outpost.coordinate.x - 1;
        int minY = outpost.coordinate.y - 1;
        int maxX = outpost.coordinate.x + 1;
        int maxY = outpost.coordinate.y + 1;

        if (minX <= 0)
            minX = 0;

        if (minY <= 0)
            minY = 0;

        if (maxX >= mapSize)
            maxX = mapSize - 1;

        if (maxY >= mapSize)
            maxY = mapSize - 1;

        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                mapData[i, j].canSpawn = CanSpawn.nothing;
            }
        }

        if (outpost.isMine)
        {
            player_OutpostList.Remove(outpost);
        }
        else
        {
            opponent_OutpostList.Remove(outpost);
        }

        foreach (Outpost playerOutpost in player_OutpostList)
        {
            SetCanSpawnPoint(playerOutpost, true);
        }

        foreach (Outpost opponentOutpost in opponent_OutpostList)
        {
            SetCanSpawnPoint(opponentOutpost, false);
        }
    }

}
