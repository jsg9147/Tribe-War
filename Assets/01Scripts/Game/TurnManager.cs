using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance
    {
        get; private set;
    }
    public static Action<bool> OnAddCard;
    public static event Action<bool> OnTurnStarted;

    public bool Tutorial;
    public float turnTime;
    public TMP_Text timeTMP;

    float currentValue;
    bool countStart = false;
    bool timeWarning = true;

    public bool myTurn;
    public bool firstTurn;

    public int turnCount;

    [Header("Properties")]
    public bool isLoading;

    WaitForSeconds delay05 = new WaitForSeconds(0.5f);


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        turnCount = 0;
    }
    private void Update()
    {
        if (countStart && !isLoading)
            TurnTimer();
    }

    public void TurnSetup(int randomTurn)
    {
        bool isServer;
        if (GameManager.instance.MultiMode)
        {
            isServer = NetworkRpcFunc.instance.isServer;
            if (randomTurn == 0)
                myTurn = isServer;
            else
                myTurn = !isServer;
        }
        else
        {
            myTurn = (randomTurn == 0);
        }

        if (CardManager.instance.TutorialGame)
        {
            myTurn = true;
        }

        StartCoroutine(StartGameCo(myTurn));
    }

    public void TurnEnd()
    {
        firstTurn = false;
        myTurn = !myTurn;
        StartCoroutine(StartTurnCo(myTurn));
        turnCount++;
    }

    public IEnumerator StartGameCo(bool playerTurn)
    {
        myTurn = playerTurn;
        delay05 = new WaitForSeconds(0.05f);
        yield return delay05;

        firstTurn = true;

        StartCoroutine(StartTurnCo(playerTurn));

        isLoading = true;
    }

    public IEnumerator StartTurnCo(bool playerTurn)
    {
        string turnMasagge = myTurn ? "Your Turn!" : "Turn End";

        isLoading = true;

        myTurn = playerTurn;
        currentValue = turnTime;
        OnTurnStarted?.Invoke(playerTurn);
        OnAddCard?.Invoke(playerTurn);

        if (myTurn)
        {
            DarkTonic.MasterAudio.MasterAudio.PlaySound("YourTurn_01");
            GameManager.instance.Notification("Your Turn!");
        }
        else
        {
            DarkTonic.MasterAudio.MasterAudio.PlaySound("EndTurn");
            GameManager.instance.Notification("Turn End");
        }

        yield return delay05;

        countStart = true;
        isLoading = false;

        GameManager.instance.EndTurnBtnSetup(myTurn);
    }

    public void StartTurn()
    {
        if (GameManager.instance.MultiMode)
        {
            GameManager.instance.localGamePlayerScript.CmdTurnEnd();
        }
        else
        {
            TurnEnd();
        }
    }

    void TurnTimer()
    {
        if (Tutorial)
            return;
        if (currentValue > 0)
        {
            currentValue -= Time.deltaTime;
            timeTMP.text = ((int)currentValue).ToString() + "s";
            if ((int)currentValue == 10 && timeWarning)
            {
                timeWarning = false;
                GameManager.instance.Notification(LocalizationManager.instance.GetIngameText("10s"));
            } 
        }
        else
        {
            timeTMP.text = "End";
            countStart = false;
            timeWarning = true;

            if (GameManager.instance.MultiMode)
            {
                if (NetworkRpcFunc.instance.isServer)
                    GameManager.instance.localGamePlayerScript.CmdTurnEnd();
            }
            else
            {
                TurnEnd();
            }
        }
    }
}
