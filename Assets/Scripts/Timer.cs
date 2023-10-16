using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        string hundreth = (Time.time % 1f).ToString("0.0");
        string seconds = ((int)(Time.time % 60)).ToString("00");
        string minutes = ((int)(Time.time / 60)).ToString("00");
        text.text = $"{minutes}:{seconds}:{hundreth.Split(",")[1]}";
    }
}
