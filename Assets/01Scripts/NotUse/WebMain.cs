using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebMain : MonoBehaviour
{
    public static WebMain instance;

    public Web web;
    public UserInfo userInfo;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        web = GetComponent<Web>();
        //userInfo = GetComponent<UserInfo>();
    }
}
