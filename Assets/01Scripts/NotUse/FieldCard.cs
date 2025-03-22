using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FieldCard : MonoBehaviour
{
    //[SerializeField] SpriteRenderer feildCardFrame;
    //[SerializeField] SpriteRenderer feildCardSprite;
    //[SerializeField] SpriteRenderer character;
    //[SerializeField] TMP_Text battlePower_TMP;
    //[SerializeField] List<SpriteRenderer> costIcons;

    //public Card card;

    //public bool isMine;
    //public bool isDie;

    //public bool attackable;

    //public Vector3 transformPos
    //{
    //    get
    //    {
    //        return transform.position;
    //    }
    //    set
    //    {
    //        MoveTransform(value, true, 0.5f);
    //    }
    //}

    //public int id;
    //public Coordinate coordinate;

    //public int liveCount;

    //public int moveCount;
    //public bool canTribute;

    //int attack, defense;


    //#region Mouse activate
    //private void OnMouseOver()
    //{
    //    if (GameManager.instance?.clickBlock ?? true)
    //        return;

    //    if (isMine)
    //        feildCardSprite.color = new Color(0.5f, 0.5f, 1f);
    //}
    //private void OnMouseExit()
    //{
    //    if (GameManager.instance?.clickBlock ?? true)
    //        return;

    //    if (isMine)
    //        feildCardSprite.color = new Color(1f, 1f, 1f);
    //}

    //private void OnMouseDown()
    //{
    //    if (GameManager.instance?.clickBlock ?? true)
    //        return;

    //    //if (isMine)
    //    //    EntityManager.instance.EntityMouseDown(this);
    //}

    //private void OnMouseUp()
    //{
    //    if (GameManager.instance?.clickBlock ?? true)
    //        return;

    //    //EntityManager.instance.EntityMouseUP(this);
    //}

    //private void OnMouseDrag()
    //{
    //    if (GameManager.instance?.clickBlock ?? true)
    //        return;

    //    //if (isMine)
    //    //    EntityManager.instance.EntityMouseDrag();
    //}
    //#endregion


    //public bool Damaged(int damage)
    //{
    //    if (GetEffectiveValue("bp") < damage)
    //    {
    //        isDie = true;
    //        return true;
    //    }
    //    return false;
    //}

    //public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTime = 0)
    //{
    //    if (useDotween)
    //        transform.DOMove(pos, dotweenTime);
    //    else
    //        transform.position = pos;

    //    moveCount++;
    //}

    //private void Start()
    //{
    //    TurnManager.OnTurnStarted += OnTurnStarted;
    //}

    //private void OnDestroy()
    //{
    //    TurnManager.OnTurnStarted -= OnTurnStarted;
    //}

    //void OnTurnStarted(bool myTurn)
    //{
    //    if (isMine == myTurn)
    //        liveCount++;

    //    card.onTurnEnd();

    //    UpdateStat();
    //}

    //public void UpdateStat()
    //{
    //    if (card.name != null && card.cardType.card_category == CardCategory.Monster)
    //    {
    //        battlePower_TMP.text = card.GetEffectiveValue("bp").ToString();
    //    }
    //}

    //public int GetEffectiveValue(string stat)
    //{
    //    return card.GetEffectiveValue(stat);
    //}

    //public void OppenentFeildCardColor(Color color)
    //{
    //    feildCardSprite.color = color;
    //}

    //public void SetGraveAfterFirst()
    //{
    //    this.transform.localScale = Vector3.zero;
    //}

    //public void Add_Apply_Effect(Ability ability)
    //{
    //    //ability.effect.Resolve(this);
    //}
}
