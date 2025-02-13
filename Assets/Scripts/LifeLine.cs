using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LifeLine : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image imgIcon;
    [SerializeField] private Image imgGlow;
    [SerializeField] private Image imgBorder;
    [SerializeField] private Image imgBackground;
    [SerializeField] private Image imgCross;
    private int state = 0;
    [SerializeField] private Lifelines lifeline;

    public void Ping()
    {
        if (state == 0)
        {
            Vector3 oldScale = imgIcon.transform.localScale;
            imgIcon.transform.DOScale(oldScale * 1.5f, 0.5f).SetEase(Ease.InOutQuad).SetLoops(2, LoopType.Yoyo);
            imgGlow.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutQuad).From(0f).SetLoops(2, LoopType.Yoyo);
            imgBackground.transform.DOScale(0.75f, 0.5f).SetEase(Ease.InOutQuad).SetLoops(2, LoopType.Yoyo);
            imgBorder.transform.DOScale(0.75f, 0.5f).SetEase(Ease.InOutQuad).SetLoops(2, LoopType.Yoyo);


            
        }
    }

    public void SetState(int state)
    {
        this.state = state;
        if (this.state < 0)
        {
            gameObject.SetActive(false);
        }
        else if (this.state > 0)
        {
            gameObject.SetActive(true);
            imgCross.gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
