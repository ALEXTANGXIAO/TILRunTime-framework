﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour//UnitySingleton<Loading>
{
    [Header("GameObjects")]
    public Text LoadingText;
    public CanvasGroup LoadingProgressBar;
    public CanvasGroup LoadingAnimation;
    public CanvasGroup LoadingCompleteAnimation;
    public static Image progressBarImage;
    private string loadingTextValue;

    void Start()
    {
        progressBarImage = LoadingProgressBar.GetComponent<Image>();
        loadingTextValue = LoadingText.text;
    }

    void Update()
    {
        
    }

    public static void SetProgressBar(float value) 
    {
        progressBarImage.fillAmount = value;
    }
}
