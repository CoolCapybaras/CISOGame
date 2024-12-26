using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public static InfoManager Instance;

    public GameObject infoObj;
    public TMP_Text text;
    
    private void Awake()
    {
        Instance = this;
    }

    public void ShowInfo(string info)
    {
        StartCoroutine(ShowInfoCoroutine(info));
    }

    private IEnumerator ShowInfoCoroutine(string info)
    {
        text.text = info;
        infoObj.SetActive(true);
        infoObj.GetComponent<CanvasGroup>().DOFade(1, 0.25f).From(0);
        yield return new WaitForSeconds(3);
        var sequence = DOTween.Sequence();
        sequence.Insert(0, infoObj.GetComponent<CanvasGroup>().DOFade(0, 0.25f).From(1))
            .InsertCallback(0.25f, () => { infoObj.SetActive(false); });
        yield return null;
    }
}