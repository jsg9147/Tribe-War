using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] TMP_Text notificationTMP;

    public void Show(string msg)
    {
        notificationTMP.text = msg;
        //Dotween ease 검색 하면 어떻게 나오는지 여러 종류가 있음
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad))
            .AppendInterval(0.5f)
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
    }

    // 유니티에서 직접 실행 가능하도록 하는 방법
    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    public void ScaleZero() => transform.localScale = Vector3.zero;

    private void Start()
    {
        ScaleZero();
    }

}
