using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeCapital_TMP : MonoBehaviour
{
    TMP_InputField inputField;
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void ChangeCapital()
    {
        inputField.text.ToUpper();
    }
}
