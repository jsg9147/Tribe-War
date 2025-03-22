using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Outpost : MonoBehaviour
{
    [SerializeField] SpriteRenderer outpost_Image;
    [SerializeField] Sprite MyOutpost;
    [SerializeField] Sprite otherPlayerOutpost;
    [SerializeField] TMP_Text LifeTMP;
    [SerializeField] GameObject OutpostObj;

    public int life;
    public bool isMine;
    public bool isDie = true;
    public bool isActive = false;

    public EntityBelong belong;

    public Vector3 transformPos
    {
        get
        {
            return new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y, 0);
        }
        set
        {
            GetComponent<Transform>().position = transformPos;
        }
    }

    public int idx;
    public Coordinate coordinate;

    public void Setup(int _life, bool _isMine, Coordinate _coordinate)
    {
        this.life = _life;
        this.isMine = _isMine;

        if (isMine)
        {
            belong = EntityBelong.Player;
        }
        else
        {
            belong = EntityBelong.Enermy;
        }

        if (_isMine)
        {
            outpost_Image.sprite = MyOutpost;
        }
        else
        {
            outpost_Image.sprite = otherPlayerOutpost;
        }
        isActive = true;

        this.coordinate = _coordinate;
    }

    public void LifeSetup()
    {
        LifeTMP.text = life.ToString();
    }

    public bool Damaged(int damage)
    {

        life -= damage;
        LifeTMP.text = life.ToString();

        if (life <= 0)
        {
            isDie = true;
            OutpostObj.SetActive(false);
            LifeTMP.text = "";
            return true;
        }
        return false;
    }
}
