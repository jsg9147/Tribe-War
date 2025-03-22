using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[System.Serializable]
public class UserInfo
{
    public string UserID;
    public string Comment;
    
    public int Win, Lose, Point, Resently_Deck_index;

    public string HaveCard_Json;

    public float WinRate()
    {
        float winRate;

        float total = Win + Lose;

        
        if (total == 0)
        {
            winRate = 0;
        }
        else
        {
            winRate = (Win / total) * 100f;
        }

        return winRate;
    }

    public void SetResult(bool isWin)
    {
        if (isWin)
        {
            Win++;
        }
        else
        {
            Lose++;
        }
    }
}
