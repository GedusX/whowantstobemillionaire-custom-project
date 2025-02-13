using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AG : LifeLineProcess
{
    // Start is called before the first frame update
    [SerializeField] private GameObject clock;
    [SerializeField] private TextMeshProUGUI txtTime;
    [SerializeField] private List<Image> clockTick;
    [SerializeField] private Image imgTick;
    [SerializeField] private RectTransform rectTick;
    [SerializeField] private Gradient gradientTick;
    [SerializeField] private RectTransform rotate;

    private IEnumerator ClockOnGoing()
    {
        AudioManager.instance.StopSound("paf_start");
        AudioManager.instance.PlaySound("paf_timer");
        _state = LifeLineState.Phase1;
        clock.SetActive(true);

        rotate.transform.DOLocalRotate(Vector3.forward * 180, 31f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        int time = 30;
        for (int i = 0; i < time; i++)
        {
            txtTime.text = (time - i).ToString();
            if (i > 0)
            {
                clockTick[i-1].color = Color.black;
            }
            yield return new WaitForSecondsRealtime(1f);
        }
        txtTime.text = (time - time).ToString();
        clockTick[time - 1].color = Color.black;
        _state = LifeLineState.End;
        
        clock.transform.DOScale(0f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            if (!AudioManager.instance.IsMusicPlaying(GameManager.instance.GetBedName(GameManager.instance.currentQuestion)))
            {
                AudioManager.instance.PlayMusic(GameManager.instance.GetBedName(GameManager.instance.currentQuestion));
            }
            EndLifeline();
        });
    }

    private void StopClock()
    {
        StopCoroutine(ClockOnGoing());
        _state = LifeLineState.Phase2;
        AudioManager.instance.StopSound("paf_timer");
        AudioManager.instance.PlaySound("paf_end");
        clock.transform.DOScale(0f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            if (!AudioManager.instance.IsMusicPlaying(GameManager.instance.GetBedName(GameManager.instance.currentQuestion)))
            {
                AudioManager.instance.PlayMusic(GameManager.instance.GetBedName(GameManager.instance.currentQuestion));
            }
            EndLifeline();
        });
    }
    public override void StartLifeline(QuestionUI questionUI)
    {
        base.StartLifeline(questionUI);
        clock.SetActive(false);
        AudioManager.instance.StopALlMusic();
        AudioManager.instance.PlaySound("paf_start");
        for (int i = 0; i < 30; i++)
        {
            clockTick.Add(Instantiate(imgTick, rectTick).GetComponent<Image>());
            clockTick.Last().transform.localPosition = new Vector2(
                205 * Mathf.Cos(Mathf.PI / 2f - (2 * Mathf.PI / 30) * i),
                205 * Mathf.Sin(Mathf.PI / 2f - (2 * Mathf.PI / 30) * i));
            clockTick.Last().color = gradientTick.Evaluate(i * 1f/30f);
            clockTick.Last().gameObject.SetActive(true);
        }
        
    }

    private void Update()
    {
        if (_state == LifeLineState.Start)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                StartCoroutine(ClockOnGoing());
            }
        }
        else if (_state == LifeLineState.Phase1)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                StopClock();
            }
        }
    }
}
