using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Rarity
{
    nomal,
    rare,
    unique
}

public class CardPack
{
    string packCode;

    List<int> nomal;
    List<int> rare;
    List<int> unique;

    public void CardPack_Data_Setup(string _code)
    {
        this.packCode = _code;

        nomal = new List<int>();
        rare = new List<int>();
        unique = new List<int>();
    }

    public string GetPackCode()
    {
        return packCode;
    }

    public void AddCard(int card_Num, int rarity)
    {
        switch (rarity)
        {
            case 1:
                nomal.Add(card_Num);
                break;
            case 2:
                rare.Add(card_Num);
                break;
            case 3:
                unique.Add(card_Num);
                break;
        }
    }

    public List<int> GetNomalList()
    {
        return nomal;
    }
    public List<int> GetRareList()
    {
        return rare;
    }

    public List<int> GetUniqueList()
    {
        return unique;
    }

    public int GetRandomNomal()
    {
        int random = Random.Range(0, nomal.Count);
        return nomal[random];
    }
    public int GetRandomRare()
    {
        int random = Random.Range(0, rare.Count);
        if (rare.Count == 0)
        {
            return GetRandomNomal();
        }
        return rare[random];
    }
    public int GetRandomUnique()
    {
        int random = Random.Range(0, unique.Count);
        if (unique.Count == 0)
        {
            return GetRandomRare();
        }
        return unique[random];
    }
}
