using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LLPanel : MonoBehaviour
{
    [SerializeField] private List<LifeLine> lifes = new List<LifeLine>();
    private float oldPosX;

    private void Start()
    {
        oldPosX = transform.localPosition.x;
    }


    public void ShowPanel()
    {
        gameObject.SetActive(true);
        transform.DOLocalMoveX(0, .5f).SetEase(Ease.OutQuad).From(2000);
        for (int i = 0; i < lifes.Count; i++)
        {
            lifes[i].SetState(GameManager.instance.lifelines[(Lifelines)i]);
        }
    }

    public void HidePanel()
    {
        transform.DOLocalMoveX(-2000f, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}
