using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerPod : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI textHeader;
    [SerializeField] private Image imgPin;
    [SerializeField] private Image imgFinal;
    [SerializeField] private Image imgCorrect;
    [SerializeField] private Color oldHeaderColor;

    private void Start()
    {
        oldHeaderColor = textHeader.color;
    }

    public void ShowAnswerPod(string answer)
    {
        text.text = answer;
        text.DOFade(1, 0.5f).SetEase(Ease.OutQuad).From(0f);
        textHeader.DOFade(1, 0.5f).SetEase(Ease.OutQuad).From(0f);
        imgPin.DOFade(1, 0.5f).SetEase(Ease.OutQuad).From(0f);
        gameObject.SetActive(true);
    }

    public void ChooseAnswerPod()
    {
        text.DOColor(Color.black, 0.5f).SetEase(Ease.OutQuad);
        textHeader.DOColor(Color.black, 0.5f).SetEase(Ease.OutQuad);
        imgFinal.DOFade(1, 0.5f).SetEase(Ease.OutQuad);
    }

    public void ResetPod()
    {
        text.color = Color.white;
        textHeader.color = oldHeaderColor;
        text.DOFade(0, 0f).SetEase(Ease.OutQuad).From(0f);
        textHeader.DOFade(0, 0f).SetEase(Ease.OutQuad).From(0f);
        imgPin.DOFade(0, 0f).SetEase(Ease.OutQuad).From(0f);
        imgFinal.DOFade(0, 0f).SetEase(Ease.InQuad);
        imgCorrect.DOFade(0, 0f).SetEase(Ease.InQuad);
        gameObject.SetActive(false);

    }
    

    public void HideAnswerPod(bool isAnimated)
    {
        if (isAnimated)
        {
            text.DOFade(0, 0.5f).SetEase(Ease.InQuad);
            textHeader.DOFade(0, 0.5f).SetEase(Ease.InQuad);
            imgPin.DOFade(0, 0.5f).SetEase(Ease.InQuad);
            imgFinal.DOFade(0, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }

    public void ShowCorrect(bool isAnimated)
    {
        text.DOColor(Color.black, 0.5f).SetEase(Ease.OutQuad);
        textHeader.DOColor(Color.black, 0.5f).SetEase(Ease.OutQuad);
        imgCorrect.DOFade(1, 0.15f).SetEase(Ease.OutQuad).From(0).SetLoops(5, LoopType.Yoyo);
        
    }
}
