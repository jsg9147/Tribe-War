using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;

public class FriendListManager : MonoBehaviour
{
    // public GameObject friendsListWindow;
    // public GameObject friendStatusObj;
    // public GameObject onlineFriendsContents;
    // public GameObject offlineFriendsContents;

    // public Button friendButton;
    // public Button onlineFriendsButton;
    // public Button offlineFriendsButton;

    // public TMP_Text connectCountText;

    // bool onlineTabActive = true;
    // bool offlineTabActive = true;
    // int onlineFriendsCount = 0;

    // List<GameObject> friendsObjList = new List<GameObject>();

    // // Start is called before the first frame update
    // void Start()
    // {
    //     friendButton.onClick.AddListener(() =>
    //     {
    //         FriendListButton();
    //     });

    //     onlineFriendsButton.onClick.AddListener(() =>
    //     {
    //         onlineTabActive = !onlineTabActive;
    //         ListViewSizeControl();
    //     });

    //     offlineFriendsButton.onClick.AddListener(() =>
    //     {
    //         offlineTabActive = !offlineTabActive;
    //         ListViewSizeControl();
    //     });

    //     CreateFriendsList();
    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    // public void CreateFriendsList()
    // {
    //     for(int i = 0; i < SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate); i++)
    //     {
    //         GameObject friendObj = Instantiate(friendStatusObj);

    //         FriendStatus friendStatus = friendObj.GetComponent<FriendStatus>();

    //         friendStatus.GetSteamID(SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate));

    //         friendsObjList.Add(friendObj);

    //         if (friendStatus.ePersonaState == EPersonaState.k_EPersonaStateOffline)
    //         {
    //             friendObj.transform.SetParent(offlineFriendsContents.transform, false);
    //         }
    //         else
    //         {
    //             friendObj.transform.SetParent(onlineFriendsContents.transform, false);
    //             onlineFriendsCount++;
    //         }
    //     }

    //     connectCountText.text = onlineFriendsCount.ToString();
    //     ListViewSizeControl();

    //     StartCoroutine(UpdateFriendStatus());
    // }

    // void FriendListButton()
    // {
    //     if(friendsListWindow.activeSelf)
    //     {
    //         friendsListWindow.SetActive(false);
    //     }
    //     else
    //     {
    //         friendsListWindow.SetActive(true);  
    //     }
    // }

    // public IEnumerator UpdateFriendStatus()
    // {
    //     yield return new WaitForSeconds(3);
    //     onlineFriendsCount = 0;
    //     foreach(var friendObj in friendsObjList)
    //     {
    //         FriendStatus friendStatus = friendObj.GetComponent<FriendStatus>();

    //         friendStatus.ChangeState();

    //         if (friendStatus.ePersonaState == EPersonaState.k_EPersonaStateOffline)
    //         {
    //             friendObj.transform.SetParent(offlineFriendsContents.transform, false);
    //         }
    //         else
    //         {
    //             onlineFriendsCount++;
    //             friendObj.transform.SetParent(onlineFriendsContents.transform, false);
    //         }
    //     }
    //     connectCountText.text = onlineFriendsCount.ToString();

    //     ListViewSizeControl();
    //     StartCoroutine(UpdateFriendStatus());
    // }

    // void ListViewSizeControl()
    // {
    //     RectTransform onlineRectTran = onlineFriendsContents.GetComponent<RectTransform>();
    //     RectTransform offlineRectTran = offlineFriendsContents.GetComponent<RectTransform>();

    //     if (onlineTabActive)
    //     {
    //         onlineFriendsContents.SetActive(true);
    //         onlineRectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, onlineFriendsContents.transform.childCount * 100);
    //     }
    //     else
    //     {
    //         onlineFriendsContents.SetActive(false);
    //     }

    //     if(offlineTabActive)
    //     {
    //         offlineFriendsContents.SetActive(true);
    //         offlineRectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, offlineFriendsContents.transform.childCount * 100);
    //     }
    //     else
    //     {
    //         offlineFriendsContents.SetActive(false);
    //     }
    // }
}
