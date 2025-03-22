using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]

public class Tile : MonoBehaviour
{
    [SerializeField] SpriteRenderer tileImage;
    public Outpost outpost;
    public GameObject outpost_object;
    Color originColor;
    Color setColor;

    public bool clickBlock = false;


    public Coordinate coordinate = new Coordinate();

    public bool canSelect;
    public CanSpawn canSpawn = CanSpawn.nothing;


    // 따로있는 이유는 outpost가 entity 분리가 안되었기 때문
    // 나중에 분리해서 하나로 만들어야함
    public TileState tileState = TileState.empty;
    public Entity onEntity;

    bool can_TileColor_Change = true;

    public bool isEmpty
    {
        get
        {
            bool isEmpty = onEntity == null && outpost.isActive == false;
            return isEmpty;
        }
    }

    public EntityBelong onTarget
    {
        get
        {
            if (onEntity != null)
            {
                return onEntity.belong;
            }
            return EntityBelong.None;
        }
    }

    public Vector3 transformPos
    {
        get
        {
            return GetComponent<Transform>().position;
        }
        set
        {
            GetComponent<Transform>().position = transformPos;
        }
    }

    public void OutpostSetActive(int life, bool isMine)
    {
        outpost.Setup(life, isMine, coordinate);
        if (isMine)
        {
            tileState = TileState.playerOutpost;
            outpost_object.SetActive(true);
            Outpost_Life_Setup();
        }
        else
        {
            tileState = TileState.enermyOutpost;
        }
    }

    public void Outpost_Life_Setup() => outpost.LifeSetup();

    public void SetOriginColor(Color color) => originColor = color;
    public void ChangeTileColor(Color color) => tileImage.material.color = color;
    public void ResetColor() => tileImage.material.color = originColor;

    public void SetupSelectOutpost(bool player)
    {
        if (player)
        {
            tileState = TileState.playerOutpost;
            DarkTonic.MasterAudio.MasterAudio.PlaySound("OutpostSetup");
        }
        else
            tileState = TileState.enermyOutpost;
    }

    #region mouse Action
    private void OnMouseEnter()
    {
        if (GameManager.instance.clickBlock)
            return;

        if (can_TileColor_Change)
        {
            if (canSelect == false)
            {
                ChangeTileColor(Color.red);
            }
            else
            {
                ChangeTileColor(Color.blue);
            }
        }
        else
        {
            ChangeTileColor(Color.blue);
        }

        ChangeTileColor(Color.blue);

        if (EntityManager.instance != null && clickBlock == false)
        {
            EntityManager.instance.selectTile = this;
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.clickBlock)
            return;
        if (clickBlock)
            return;

        MapManager.instance?.SetupOutpost(this);
        MapManager.instance?.Select_Effect_Tile(this);
    }

    private void OnMouseExit()
    {
        if (GameManager.instance.clickBlock)
            return;

        if (can_TileColor_Change)
        {
            if (canSelect == false)
            {
                ChangeTileColor(Color.red);
            }
            else
            {
                ResetColor();
                if (EntityManager.instance != null)
                {
                    EntityManager.instance.selectTile = null;
                }
            }
        }
        else
        {
            ChangeTileColor(setColor);
        }
        if (EntityManager.instance != null)
        {
            EntityManager.instance.selectTile = null;
        }
    }
    #endregion

    public void ColorChange_Rock(bool changeColor, Color color = new Color())
    {
        if (changeColor)
        {
            ChangeTileColor(color);
            setColor = color;
            can_TileColor_Change = false;
        }
        else
        {
            ResetColor();
            can_TileColor_Change = true;
        }
    }
}
