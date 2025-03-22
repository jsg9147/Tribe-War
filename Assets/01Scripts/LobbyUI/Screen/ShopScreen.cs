using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ShopScreen : MonoBehaviour
{
    public Transform shopContent;
    public CardPackBuy cardPackPrefab;

    Dictionary<string, string> packInfo;

    public GameObject drawCastScreen;
    public Transform card_Layout_group;

    public GameObject[] SlotObject;
    Vector3[] slotOriginPos;

    //private void Start()
    //{
    //    slotOriginPos = new Vector3[SlotObject.Length];

    //}
    //void Update()
    //{

    //}

    //private void OnEnable()
    //{
    //    Pack_Data_Setup();
    //}

    //void Pack_Data_Setup()
    //{
    //    packInfo = new Dictionary<string, string>();
    //    this.packInfo = WebMain.instance.web.packInfo;
    //    foreach (var pack in packInfo.Keys)
    //    {
    //        CardPackBuy cardPackBuy = Instantiate(cardPackPrefab, shopContent);
    //        cardPackBuy.transform.localScale = Vector3.one;
    //        cardPackBuy.transform.SetParent(shopContent);
    //        CardPack cardPack = WebMain.instance.web.cardPacks.Find(x => x.GetPackCode() == pack);
    //        cardPackBuy.Pack_Setup(cardPack, packInfo[pack], this);
    //    }
    //}

    //public void DrawScreen_On()
    //{
    //    drawCastScreen.transform.DOScale(Vector3.one, 1);
    //    for (int i = 0; i < slotOriginPos.Length; i++)
    //    {
    //        slotOriginPos[i] = SlotObject[i].transform.localPosition;
    //    }
    //}

    //public void DrawScreen_Off()
    //{
    //    for (int i = 0; i < slotOriginPos.Length; i++)
    //    {
    //        SlotObject[i].transform.localPosition = slotOriginPos[i];
    //    }
    //    drawCastScreen.transform.DOKill();
    //    drawCastScreen.transform.localScale = Vector3.zero;
    //    DrawReset();
    //}

    //void DrawReset()
    //{
    //    Transform[] childrenList = card_Layout_group.GetComponentsInChildren<Transform>();

    //    if (childrenList != null)
    //    {
    //        for (int i = 1; i < childrenList.Length; i++)
    //        {
    //            if (childrenList[i] != transform)
    //                Destroy(childrenList[i].gameObject);
    //        }
    //    }
    //}


    //public IEnumerator StartSlot(int SlotIndex, int cycle, int index) // 변수로 바퀴수, 인덱스 추가해서
    //{
    //    Vector3 slotPos = SlotObject[SlotIndex].transform.localPosition;
    //    Vector3 destinationPos = slotPos + new Vector3(0, -250 * index);

    //    float randSpeed = Random.Range(0.002f, 0.005f);
    //    for (int i = 0; i < 223 * cycle; i++) // 
    //    {
    //        SlotObject[SlotIndex].transform.localPosition -= new Vector3(0, 10f, 0);
    //        if (SlotObject[SlotIndex].transform.localPosition.y <= -2110f)
    //        {
    //            SlotObject[SlotIndex].transform.localPosition = slotPos;
    //        }
    //        yield return new WaitForSeconds(randSpeed);
    //    }
    //    SlotObject[SlotIndex].transform.localPosition = slotPos + new Vector3(0, 40);

    //    SlotObject[SlotIndex].transform.DOLocalMove(destinationPos, 1f).SetEase(Ease.Linear);
    //}

    // 샵 기능이 지금 사라지게 되었다.. 나중에 다시 만들자
    // 10개 카드 랜덤으로 띄우고
    // 뽑힌 카드 위치의 인덱스를 받고
    // 랜덤하게 몇바퀴 이상에서 멈추게 하고 그 위치에서 멈추게 만든다.
    // 시작점을 당첨카드로 놓고 돌리는 바퀴수를 랜덤에 시간도 랜덤으로 한다..
    // 한바퀴는 온전히 돌리고 마지막 바퀴에서 반복문을 나와서 그 인덱스의 좌표로 이동
}
