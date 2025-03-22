using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Steamworks;
using Mirror;

using Random = UnityEngine.Random;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public static CardManager instance
    {
        get; private set;
    }

    public bool TutorialGame;
    int tutorial_index = 0;
    
    public Canvas canvas;
    public EffectManager effectManager;
    public GraveManager graveManager;

    [Header("Deck remain card List")]
    public CardPreview deckCard;
    public Transform deck_Content;

    [Header("Object")]
    [SerializeField] CameraEffect cameraEffect;
    [SerializeField] GameObject handPrefab;
    [SerializeField] GameObject playerCardParent;
    [SerializeField] GameObject opponentCardParent;

    [SerializeField] CardUI EnlargeEffectCard;

    [Header("Card Spawn Position")]
    [SerializeField] Transform playerCardSpawnPoint;
    [SerializeField] Transform opponentCardSpawnPoint;

    [Header("Card Alignment")]
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] Transform otherCardLeft;
    [SerializeField] Transform otherCardRight;
    [SerializeField] EcardState ecardState;

    [SerializeField] TMPro.TMP_Text playerDeckCounting;
    [SerializeField] TMPro.TMP_Text opponentDeckCounting;

    public List<Hand> playerCards;
    public List<Hand> opponentCards;

    public int maxHandCard;
    public bool reloading;
    public bool selectState => selectCard != null;

    public TMPro.TMP_Text deckCount_text;

    List<Card> itemBuffer;
    List<Card> aI_ItemBuffer;
    List<CardPreview> viewList;

    Dictionary<string, int> cardCounting;

    Hand selectCard = null;

    bool isMyCardDrag;

    int useCardIndex;

    Hand tempTributeCard;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (isMyCardDrag)
            CardDrag();

        SetEcardState();
    }
    private void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;
        TurnManager.OnTurnStarted -= TurnEndSetup;
    }

    void Init()
    {
        playerCards = new List<Hand>();
        opponentCards = new List<Hand>();
        cardCounting = new Dictionary<string, int>();
        reloading = false;

        TurnManager.OnAddCard += AddCard;
        TurnManager.OnTurnStarted += TurnEndSetup;
        Playing_Deck_Init();

        if (AIManager.instance.SinglePlay)
        {
            AI_SetupItemBuffer();
        }
    }

    void Playing_Deck_Init()
    {
        SetupItemBuffer();
        viewList = new List<CardPreview>();

        for (int i = 0; i < itemBuffer.Count; i++)
        {
            if (cardCounting.ContainsKey(itemBuffer[i].id))
            {
                cardCounting[itemBuffer[i].id]++;
            }
            else
            {
                cardCounting.Add(itemBuffer[i].id, 1);
            }
        }

        foreach (string card_id in cardCounting.Keys)
        {
            CardPreview cardPreview = Instantiate(deckCard, deck_Content);
            cardPreview.Setup(DataManager.instance.CardData(card_id), cardCounting[card_id]);
            cardPreview.SetEnlargeCardManager(EnlargeCardManager.instance);
            viewList.Add(cardPreview.GetComponent<CardPreview>());
        }
        Deck_Item_Update();
        deck_Content.transform.GetComponent<FlexibleGrid>().SetFlexibleGrid();
    }
    void SetupItemBuffer()
    {
        List<string> deckItem = DataManager.instance.Select_Deck.cards;

        itemBuffer = new List<Card>();

        foreach (string card_id in deckItem)
        {
            Card card = DataManager.instance.CardData(card_id);

            itemBuffer.Add(card);
        }

        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            Card temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
        playerDeckCounting.text = itemBuffer.Count.ToString();
    }

    void AI_SetupItemBuffer()
    {
        aI_ItemBuffer = DataManager.instance.AI_CardList();

        for (int i = 0; i < aI_ItemBuffer.Count; i++)
        {
            int rand = Random.Range(i, aI_ItemBuffer.Count);
            Card temp = aI_ItemBuffer[i];
            aI_ItemBuffer[i] = aI_ItemBuffer[rand];
            aI_ItemBuffer[rand] = temp;
        }
    }
    void Deck_Item_Update()
    {
        for (int i = 0; i < viewList.Count; i++)
        {
            viewList[i].Setup(viewList[i].card, cardCounting[viewList[i].card.id]);
            if (viewList[i].count <= 0)
            {
                viewList[i].gameObject.SetActive(false);
            }
            else
            {
                viewList[i].gameObject.SetActive(true);
            }
        }
        viewList.OrderBy(x => x.card.id).ThenBy(x => x.card.name);
        deckCount_text.text = itemBuffer.Count.ToString();
    }

    Card PopItem(bool isMine)
    {
        if (isMine)
        {
            if (TutorialGame)
            {
                if (tutorial_index >= 18)
                {
                    tutorial_index = 0;
                }
                Card tutorialCard = DataManager.instance.Tutorial_Card(tutorial_index);
                tutorial_index++;
                return tutorialCard;
            }

            Card card = itemBuffer[0];
            itemBuffer.RemoveAt(0);

            if (GameManager.instance.MultiMode)
            {
                GameManager.instance.localGamePlayerScript.CmdSetDeckCounting(itemBuffer.Count);
            }
            else
            {
                Deck_Counting_Update(isMine, itemBuffer.Count);
            }

            Deck_Item_Update();
            return card;
        }
        else
        {
            if (AIManager.instance.SinglePlay)
            {
                Card ai_Card = aI_ItemBuffer[0];
                aI_ItemBuffer.RemoveAt(0);

                return ai_Card;
            }
        }

        return null;
    }

    public void Deck_Counting_Update(bool isMine, int deckCount)
    {
        if (isMine)
            playerDeckCounting.text = itemBuffer.Count.ToString();
        else
            opponentDeckCounting.text = deckCount.ToString();
    }

    public void MouseEventInit()
    {
        //isMyCardDrag = false;
        selectCard?.MoveTransform(selectCard.originPRS, false);
    }

    public void HandBlock(bool block)
    {
        foreach (Hand hand in playerCards)
        {
            hand.clickBlock = block;
        }
    }

    
    public void Can_Use_Effect(bool isActive = true)
    {
        if (reloading)
            return;

        if (isActive)
        {
            foreach (Hand hand in playerCards)
            {
                hand.Can_Use_Effect(EntityManager.instance.canUseCard(true, hand.card));
            }
        }
        else
        {
            foreach (Hand hand in playerCards)
            {
                hand.Can_Use_Effect(false);
            }
        }
    }

    void AddCard(bool isMine)
    {
        GameObject parent = isMine ? playerCardParent : opponentCardParent;
        Transform cardSpawnPoint = isMine ? playerCardSpawnPoint : opponentCardSpawnPoint;
        List<Hand> targetHand = isMine ? playerCards : opponentCards;

        if (AIManager.instance.SinglePlay)
        {
            if (TurnManager.instance.firstTurn)
            {
                ReloadCard(!isMine);
                CardAlignment(!isMine, 0.8f);
            }
        }

        if (targetHand.Count == maxHandCard && TurnManager.instance.turnCount > 0)
        {
            if (GameManager.instance.MultiMode)
            {
                if (isMine)
                {
                    GameManager.instance.localGamePlayerScript.CmdReloadHandCard(NetworkRpcFunc.instance.isServer);
                }
            }
            else
            {
                ReloadCard(isMine);
            }
            CardAlignment(isMine, 0.8f);
            return;
        }

        if (targetHand.Count < maxHandCard)
        {
            if (isMine || AIManager.instance.SinglePlay)
            {
                for (int i = targetHand.Count; i < maxHandCard; i++)
                {
                    if (itemBuffer.Count == 0 && isMine)
                    {
                        if (TutorialGame == false)
                        {
                            GameManager.instance.GameResult(false, NetworkRpcFunc.instance.isServer);
                            break;
                        }
                    }
                    GameObject cardObject = Instantiate(handPrefab, cardSpawnPoint.position, Utils.QI);
                    cardObject.SetActive(true);
                    cardObject.transform.SetParent(parent.transform);
                    cardObject.transform.localScale = Vector3.one;

                    Hand card = cardObject.GetComponent<Hand>();
                    card.Setup(PopItem(isMine), isMine);
                    cardObject.name = card.card.name;
                    targetHand.Add(card);

                    DarkTonic.MasterAudio.MasterAudio.PlaySound("Deal_Single_Whoosh_01");
                }
            }
            else
            {
                for (int i = targetHand.Count; i < maxHandCard; i++)
                {
                    GameObject cardObject = Instantiate(handPrefab, cardSpawnPoint.position, Utils.QI);
                    cardObject.SetActive(true);
                    cardObject.transform.SetParent(parent.transform);
                    cardObject.transform.localScale = Vector3.one;

                    Hand card = cardObject.GetComponent<Hand>();
                    card.Setup(null, isMine);
                    targetHand.Add(card);

                    DarkTonic.MasterAudio.MasterAudio.PlaySound("Deal_Single_Whoosh_01");
                }
            }
        }

        CardAlignment(isMine, 0.8f);
    }

    public void StartCardDealing()
    {
        StartCoroutine(ReloadHandCard_Coroutine(true));
        StartCoroutine(ReloadHandCard_Coroutine(false));
    }

    public void ReloadCard(bool server)
    {
        bool isMine = GameManager.instance.IsMine(server);
        StartCoroutine(ReloadHandCard_Coroutine(isMine));
    }

    public IEnumerator ReloadHandCard_Coroutine(bool isMine)
    {
        bool singlePlay = AIManager.instance.SinglePlay;

        List<Hand> targetCards = isMine ? playerCards : opponentCards;
        Transform cardSpawnPoint = isMine ? playerCardSpawnPoint : opponentCardSpawnPoint;
        GameObject cardObjectParent = isMine ? playerCardParent : opponentCardParent;

        reloading = true;

        foreach (Hand handCard in targetCards)
        {
            DarkTonic.MasterAudio.MasterAudio.PlaySound("Deal_Single_Whoosh_02");
            handCard.transform.DOKill(true);
            if (isMine)
            {
                Card reloadCard = new Card(handCard.card);
                itemBuffer.Add(reloadCard);
            }
            else if (singlePlay)
            {
                Card reloadCard = new Card(handCard.card);
                aI_ItemBuffer.Add(reloadCard);
            }

            handCard.transform.DOMove(cardSpawnPoint.transform.position, 0.2f);
            yield return new WaitForSeconds(0.2f);
            handCard.transform.DOKill(true);
            Destroy(handCard.gameObject);
        }

        targetCards.Clear();

        for (int i = targetCards.Count; i < maxHandCard; i++)
        {
            if (isMine)
            {
                if (itemBuffer.Count == 0)
                    break;
            }
            else if (singlePlay)
            {
                if (aI_ItemBuffer.Count == 0)
                {
                    AI_SetupItemBuffer();
                }
            }

            GameObject cardObject = Instantiate(handPrefab, cardSpawnPoint.position, Utils.QI);
            cardObject.transform.SetParent(cardObjectParent.transform);
            cardObject.transform.localScale = Vector3.one;

            Hand handCard = cardObject.GetComponent<Hand>();
            handCard.Setup(PopItem(isMine), isMine);

            cardObject.name = handCard.card.name;
            targetCards.Add(handCard);
            handCard.gameObject.SetActive(true);
            DarkTonic.MasterAudio.MasterAudio.PlaySound("Deal_Single_Whoosh_01");
        }

        CardAlignment(isMine, 0.8f);

        reloading = false;
    }

    void TestAligment(float alignmentTIme = 0f)
    {
        var originCardPRSs = RoundAlignment(myCardLeft, myCardRight, playerCards.Count, 0.5f, Vector3.one);
        int targetIndex = playerCards.FindIndex(x => x == selectCard);

        if (targetIndex == -1)
            return;

        var targetHand = playerCards[targetIndex];
        targetHand.originPRS = originCardPRSs[targetIndex];
        targetHand.MoveTransform(targetHand.originPRS, true, alignmentTIme);
        Can_Use_Effect();
    }

    //test로 다 바꿔보기 지금은 귀찮아서..

    void CardAlignment(bool isMine, float alignmentTIme = 0f)
    {
        List<PRS> originCardPRSs = new List<PRS>();
        if (isMine)
        {
            originCardPRSs = RoundAlignment(myCardLeft, myCardRight, playerCards.Count, 0.5f, Vector3.one);
        }
        else
        {
            originCardPRSs = RoundAlignment(otherCardLeft, otherCardRight, opponentCards.Count, -0.5f, Vector3.one);
        }

        var targetCards = isMine ? playerCards : opponentCards;
        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];
            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, alignmentTIme);
        }

        Can_Use_Effect();
    }

    // jsg 카드 정렬함수, 문제 생길수 있으니 생기면 찾아봐야함
    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> result = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.identity;

            result.Add(new PRS(targetPos, targetRot, scale));
        }

        return result;
    }

    private void CardDrag()
    {
        if (selectCard != null)
        {
            selectCard.MoveTransform(new PRS(new Vector3(Utils.MousePos.x, Utils.MousePos.y + 15, Utils.MousePos.z), Utils.QI, selectCard.originPRS.scale), false);
            selectCard.modulateAlpha(0.5f);
        }
    }

    public void TryPutCard(bool server, string card_id, Coordinate selectCoord)
    {
        bool isMine = GameManager.instance.IsMine(server);
        
        if (card_id == null || EntityManager.instance.SelectMonsterMode) { return; }
        List<Hand> targetList = isMine ? playerCards : opponentCards;
        Hand handCard = targetList.Find(x => x.card.id == card_id);
        Card tryCard = DataManager.instance.CardData(card_id);

        tempTributeCard = handCard;

        if (isMine)
        {
            useCardIndex = targetList.IndexOf(handCard);
        }
        else
        {
            if (!GameManager.instance.MultiMode)
            {
                useCardIndex = targetList.IndexOf(handCard);
            }
        }

        if (tryCard.cardType.card_category == CardCategory.Monster)
        {
            EntityManager.instance.Summon(isMine, card_id, selectCoord);
        }
        else
        {
            if (effectManager.EffectTrigger(isMine, card_id))
            {
                if (isMine)
                {
                    targetList.Remove(handCard);
                    handCard.transform.DOKill();
                    Destroy(handCard.gameObject);
                    selectCard = null;
                }
                else
                {
                    RemoveTargetCards(isMine);
                }

                if (tryCard.cardType.card_category != CardCategory.Monster)
                {
                    EffectCard_Animation(tryCard, isMine);
                    DarkTonic.MasterAudio.MasterAudio.PlaySound("Magic01");
                }

                CardAlignment(isMine);
                EnlargeCardManager.instance.Setup(tryCard, true);
            }
            else
            {
                CardAlignment(isMine);
            }
        }

        if (cardCounting.ContainsKey(card_id))
        {
            cardCounting[card_id]--;
        }
    }

    void EffectCard_Animation(Card triggerCard, bool isMine)
    {
        CardUI triggerCardUI = Instantiate(EnlargeEffectCard, canvas.transform);
        triggerCardUI.Setup(triggerCard);

        Sequence sequence = DOTween.Sequence()
        .Append(triggerCardUI.transform.DOMoveX(-120f, 0.8f))
        .AppendCallback(() =>
        {
            graveManager.AddGraveCard(triggerCard, isMine);
        }).OnComplete(() => Destroy(triggerCardUI));
        triggerCardUI.transform.DOScale(Vector3.zero, 0.8f);
    }

    // EntityManager의 Summon 에서 제물소환시 카드를 숨겼다 하면 다시 보이게 하려고 만듬 함수
    public void TributeSummonSet(bool tributeSummon)
    {
        if (tempTributeCard == null)
            return;

        tempTributeCard.gameObject.SetActive(tributeSummon);

        if (tributeSummon)
        {
            tempTributeCard = null;
        }
    }

    void TurnEndSetup(bool myTurn)
    {
        if (myTurn == false && tempTributeCard != null)
            tempTributeCard.gameObject.SetActive(true);

    }

    public void Remove_OhterPlayer_HandCard()
    {
        Hand card = opponentCards[Random.Range(0, opponentCards.Count)];
        card.transform.DOKill();

        opponentCards.Remove(card);
        Destroy(card.gameObject);

        CardAlignment(false);
    }

    public void SetMostFrontOrderInit(bool server)
    {
        bool isMine = server == NetworkRpcFunc.instance.isServer;
        //var targetList = isMine ? playerCards : opponentCards;
        //targetList.ForEach(x => x.GetComponent<Order>().SetMostFrontOrder(false));
        CardAlignment(isMine);
    }

    public void RemoveTargetCards(bool isMine)
    {
        List<Hand> targetHands = isMine ? playerCards : opponentCards;
        Hand card;

        if (reloading)
            return;

        
        if (GameManager.instance.MultiMode && isMine == false)
        {
            card = targetHands[Random.Range(0, targetHands.Count)];
        }
        else
        {
            card = targetHands[useCardIndex];
        }
        targetHands.Remove(card);
        card.transform.DOKill();
        Destroy(card.gameObject);
        if (isMine)
        {
            selectCard = null;
        }
        CardAlignment(isMine);

        useCardIndex = 0;
    }

    #region MyCard

    public void CardMouseOver(Hand card)
    {
        if (ecardState == EcardState.Nothing)
            return;

        if (isMyCardDrag)
            return;

        selectCard = card;
        EnlargeCard(true, card);

        DarkTonic.MasterAudio.MasterAudio.PlaySound("CardMouseOver");
    }

    public void CardMouseExit(Hand card)
    {
        EnlargeCard(false, card);

        if (isMyCardDrag)
            return;

        selectCard = null;
    }

    public void CardMouseDown(Hand card)
    {
        if (ecardState != EcardState.CanMouseDrag)
            return;

        isMyCardDrag = true;
    }

    public void CardMouseUp(Hand handCard)
    {
        isMyCardDrag = false;

        if (ecardState != EcardState.CanMouseDrag)
            return;

        CardAlignment(true);

        if (EntityManager.instance.selectTile != null)
        {
            if (selectCard != null)
            {
                if (GameManager.instance.MultiMode)
                {
                    GameManager.instance.localGamePlayerScript.CmdTryPutCard(NetworkRpcFunc.instance.isServer, handCard.card.id, EntityManager.instance.selectTile.coordinate.vector3Pos);
                }
                else
                {
                    TryPutCard(true, handCard.card.id, EntityManager.instance.selectTile.coordinate);
                }
            }
        }
    }

    void EnlargeCard(bool isEnlarge, Hand card)
    {
        if (reloading)
        {
            return;
        }
        // card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, card.originPRS.pos.y + 50, 0f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 1.5f), false);
            card.transform.SetAsLastSibling();
            card.modulateAlpha(0.5f);
        }
        else
        {
            TestAligment();
            card.modulateAlpha(1f);
            //CardAlignment(true);
        }
    }

    private void SetEcardState()
    {
        if (TurnManager.instance == null)
        {
            return;
        }

        if (TurnManager.instance.isLoading)
            ecardState = EcardState.Nothing;

        else if (TurnManager.instance.myTurn == false) // 기존 코드에서 내턴 카드 제한을 풀었음
            ecardState = EcardState.CanMouseOver;

        else if (TurnManager.instance.myTurn)
            ecardState = EcardState.CanMouseDrag;
    }

    #endregion
}
