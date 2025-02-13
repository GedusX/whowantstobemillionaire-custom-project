using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DD : LifeLineProcess
{
    [SerializeField] private GameObject ll;
    private bool secondChance = false;
    private int chosen = -1;
    private Question question;
    private List<AnswerPod> answerPods;
    [SerializeField] private KeyCode[] ansKey = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };


    public override void StartLifeline(QuestionUI questionUI)
    {
        base.StartLifeline(questionUI);
        AudioManager.instance.StopALlMusic();
        AudioManager.instance.PlaySound("dd_start");
        question = questionUI.GetCurrentQuestion;
        answerPods = this.questionUI.GetPossibleAnswers;
        ll.transform.DOScale(1, 0.5f).SetEase(Ease.OutQuad).From(0f).OnComplete(() =>
        {
            ll.GetComponent<LifeLine>().Ping();
        });
    }


    public void Correct()
    {
        //correct Ans
        answerPods[chosen].ShowCorrect(true);
        _state = LifeLineState.End;
        GameManager.OnCorrect.Invoke(GameManager.instance.currentQuestion);

        ll.transform.DOScale(0f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            questionUI.SetState(QState.Correct);
            EndLifeline(false);
        });
    }

    public void Wrong()
    {
        if (secondChance)
        {
            answerPods[question.correctAnswer].ShowCorrect(true);
            _state = LifeLineState.End;
            AudioManager.instance.StopSound("dd_secondChance");
            AudioManager.instance.StopSound("dd_choosing");
            GameManager.OnIncorrect.Invoke(GameManager.instance.currentQuestion);
            ll.transform.DOScale(0f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                questionUI.SetState(QState.Incorrect);
                EndLifeline(false);
            });
        }
        else
        {
            answerPods[chosen].HideAnswerPod(true);
            ll.GetComponent<LifeLine>().Ping();
            secondChance = true;
            _state = LifeLineState.Phase2;
            AudioManager.instance.PlaySound("dd_secondChance");
            AudioManager.instance.StopSound("dd_choosing");
            AudioManager.instance.StopSound("dd_start");


        }
        
    }

    private void Update()
    {
        if (_state == LifeLineState.Start || _state == LifeLineState.Phase2)
        {
            for (int i = 0; i < ansKey.Length; i++)
            {
                if (Input.GetKeyDown(ansKey[i]) && answerPods[i].gameObject.activeSelf)
                {
                    answerPods[i].ChooseAnswerPod();
                    chosen = i;
                    AudioManager.instance.PlaySound("dd_choose");
                    if (secondChance == false)
                    {
                        _state = LifeLineState.Phase1;
                    }
                    else
                    {
                        _state = LifeLineState.Phase3;
                    }
                }
            }
        }
        else if (_state == LifeLineState.Phase1 || _state == LifeLineState.Phase3)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (chosen != question.correctAnswer)
                {
                    Wrong();
                }
                else
                {
                    Correct();
                }
                
            }
        }
    }
}
