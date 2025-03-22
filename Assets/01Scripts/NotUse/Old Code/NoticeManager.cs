using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeManager : MonoBehaviour
{
    // [SerializeField] List<Sprite> notice;
    // [SerializeField] ToggleGroup toggleGroup;
    // [SerializeField] GameObject toggleObj;
    // [SerializeField] Image noticeImage;

    // List<Toggle> toggles;
    // int activeToggleNumber;
    // private void Start()
    // {
    //     NoticeArrange();
    // }

    // void NoticeArrange()
    // {
    //     toggles = new List<Toggle>();
    //     foreach (var noticeImg in notice)
    //     {
    //         var toggleObject = Instantiate(toggleObj, toggleGroup.transform);

    //         Toggle toggle = toggleObject.GetComponent<Toggle>();
    //         toggle.group = toggleGroup;
    //         toggles.Add(toggle);

    //         toggle.onValueChanged.AddListener((bool isOn) =>
    //         {
    //             noticeImage.sprite = noticeImg;
    //             activeToggleNumber = toggles.FindIndex(t => t == toggle);
    //         });
    //     }
    //     toggles[0].isOn = true;

    //     StartCoroutine(NoticeTurnOver());
    // }

    // IEnumerator NoticeTurnOver()
    // {
    //     yield return new WaitForSecondsRealtime(5f);

    //     activeToggleNumber++;

    //     if (activeToggleNumber == toggles.Count)
    //     {
    //         activeToggleNumber = 0;
    //     }
    //     toggles[activeToggleNumber].isOn = true;

    //     StartCoroutine(NoticeTurnOver());
    // }
}
