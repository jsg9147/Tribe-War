using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Hand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Sprite RookFrame;
    public Sprite BishopFrame;
    public Sprite QueenFrame;
    public Sprite MagicFrame;
    public Sprite BackFrame;

    public List<Sprite> Level_Sprite;

    [SerializeField] Image cardFrame;
    [SerializeField] Image classIcon;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] Image cardImage;
    [SerializeField] Image level_Icon;

    [SerializeField] TMP_Text tribeTextTmp;
    [SerializeField] TMP_Text cardTextTmp;
    [SerializeField] TMP_Text BattlePointTMP;

    [SerializeField] GameObject possible_Effect;

    public Card card;
    bool isFront;
    public PRS originPRS;

    public bool clickBlock;

    public void Setup(Card card, bool isFront)
    {
        this.isFront = isFront;

        if (card == null && isFront)
            return;

        if (this.isFront)
        {
            this.card = card;
            cardImage.sprite = card.sprite;
            nameTMP.text = card.name;
            if (card.cardType.card_category == CardCategory.Monster)
            {
                BattlePointTMP.text = "BP : " + card.GetBaseStat("bp").ToString();

                if (card.cardType.moveType == MoveType.Rook)
                    cardFrame.sprite = RookFrame;
                else if (card.cardType.moveType == MoveType.Bishop)
                    cardFrame.sprite = BishopFrame;
                else if (card.cardType.moveType == MoveType.Queen)
                    cardFrame.sprite = QueenFrame;
            }
            else
            {
                BattlePointTMP.text = "";

                cardFrame.sprite = MagicFrame;
            }
            try
            {
                cardTextTmp.text = card.text.ToString();
                classIcon.sprite = card.cardType.typeIcon;
                tribeTextTmp.text = "[" + DataManager.instance.tribeStr[card.cardType.tribe] + "]";
            }
            catch(System.NullReferenceException ex)
            {
                Debug.Log(ex);  
            }
            level_Icon.sprite = Level_Sprite[card.cost];

            clickBlock = false;
        }
        else
        {
            if (card != null)
                this.card = card;

            cardFrame.sprite = BackFrame;
            classIcon.gameObject.SetActive(false);
            nameTMP.gameObject.SetActive(false);
            cardImage.gameObject.SetActive(false);
            cardTextTmp.gameObject.SetActive(false);
            BattlePointTMP.gameObject.SetActive(false);
            possible_Effect.SetActive(false);

            clickBlock = true;
        }
    }
    string tribeStr(Tribe tribe)
    {
        switch (tribe)
        {
            case Tribe.Dragon:
                return "[Dragon]";
            case Tribe.Warrior:
                return "[Warrior]";
            case Tribe.Magician:
                return "[Magician]";
            default:
                return "[Common]";
        }
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

    public void modulateAlpha(float value)
    {
        Color alpha = Color.white;

        alpha.a = value;

        cardFrame.GetComponent<Image>().color = alpha;
        cardImage.GetComponent<Image>().color = alpha;
    }

    public void changeColor(Color changeColor)
    {
        cardFrame.color = changeColor;
    }

    public void Can_Use_Effect(bool isActive)
    {
        possible_Effect?.SetActive(isActive);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.instance?.clickBlock ?? true)
            return;
        if (clickBlock)
            return;

        if (isFront)
        {
            if (CardManager.instance != null)
                CardManager.instance.CardMouseOver(this);

            Color color = new Color32(146, 233, 255, 255);

            changeColor(color);


        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.instance?.clickBlock ?? true)
            return;
        if (clickBlock)
            return;

        if (isFront)
        {
            if (CardManager.instance != null)
                CardManager.instance.CardMouseExit(this);

            changeColor(Color.white);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.instance?.clickBlock ?? true)
            return;
        if (clickBlock)
            return;

        if (isFront)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                EnlargeCardManager.instance.Setup(card, isFront);
                return;
            }

            if (CardManager.instance != null)
                CardManager.instance.CardMouseDown(this);

            changeColor(Color.white);
            modulateAlpha(0.5f);
            possible_Effect?.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.instance?.clickBlock ?? true)
            return;
        if (clickBlock)
            return;

        if (isFront)
        {
            modulateAlpha(1f);
            if (CardManager.instance != null)
                CardManager.instance.CardMouseUp(this);
        }
    }
}
