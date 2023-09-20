using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ActionBusyUI : MonoBehaviour
{
    [SerializeField] private Image backImage;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float fadeSpeed = 2f;
    private float currentAlpha;

    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        Hide();
    }
    private void Update()
    {
        currentAlpha = backImage.color.a;
    }
    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        StopAllCoroutines();
        if (isBusy)
            Show();
        else
            Hide();
    }
    private void Show()
    {
        StartCoroutine(Fade(1f, true));
    }

    private void Hide()
    {
        StartCoroutine(Fade(0f, false));
    }

    private IEnumerator Fade(float tagetAlpha, bool isShowing)
    {
        text.text = "";
        if (!isShowing)
            text.text = "";
        while (!Mathf.Approximately(currentAlpha, tagetAlpha))
        {
            float alpha = Mathf.MoveTowards(currentAlpha, tagetAlpha, fadeSpeed * Time.deltaTime);
            backImage.color = new Color(backImage.color.r, backImage.color.g, backImage.color.b, alpha);
            yield return null;
        }
        if (isShowing)
            text.text = "行动中";

    }
}
