using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;

public class PlayerProfile : MonoBehaviour
{
    public Image userIcon;
    public TMP_Text username_Text;
    public TMP_Text playerRecord_Text;
    public Image background;
    public GameObject readyImage;

    public void PlayerProfileUpdate(float win, float lose, float winRate ,string steamName, int iImage, bool hasAuthority)
    {
        string winRateStr = winRate.ToString("F1");

        username_Text.text = steamName;
        playerRecord_Text.text = (int)win + "W " + (int)lose + "L ( " + winRateStr + " % )";

        Texture2D steamAvatar = GetSteamImageAsTexture2D(iImage);

        if (hasAuthority)
            background.color = new Color(0.8f, 0.5f, 0.2f, 1);

        if (steamAvatar != null)
        {
            Rect rect = new Rect(0, 0, steamAvatar.width, steamAvatar.height);
            userIcon.sprite = Sprite.Create(steamAvatar, rect, new Vector2(0.5f, 0.5f));
        }
    }
    public void SetProfileIcon(int iImage)
    {
        Texture2D steamAvatar = GetSteamImageAsTexture2D(iImage);

        if (steamAvatar == null)
            return;

        Rect rect = new Rect(0, 0, steamAvatar.width, steamAvatar.height);
        userIcon.sprite = Sprite.Create(steamAvatar, rect, new Vector2(0.5f, 0.5f));
    }
    public Texture2D GetSteamImageAsTexture2D(int iImage)
    {
        Texture2D ret = null;
        uint ImageWidth;
        uint ImageHeight;
        bool bIsValid = SteamUtils.GetImageSize(iImage, out ImageWidth, out ImageHeight);

        if (bIsValid)
        {
            byte[] Image = new byte[ImageWidth * ImageHeight * 4];

            bIsValid = SteamUtils.GetImageRGBA(iImage, Image, (int)(ImageWidth * ImageHeight * 4));
            if (bIsValid)
            {
                ret = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, false);
                ret.LoadRawTextureData(Image);

                ret.Apply();
            }
        }
        return ret;
    }

    public void ReadyState(bool ready)
    {
        readyImage.SetActive(ready);
    }
}
