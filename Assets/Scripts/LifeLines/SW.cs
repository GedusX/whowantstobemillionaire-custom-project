using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SW : LifeLineProcess
{
    [SerializeField] private GameObject ll;
    private bool secondChance = false;
    private int chosen = -1;
    private Question question;
    private List<AnswerPod> answerPods;


    public override void StartLifeline(QuestionUI questionUI)
    {
        base.StartLifeline(questionUI);
        AudioManager.instance.StopALlMusic();
        AudioManager.instance.PlaySound("sw_start");
        question = questionUI.GetCurrentQuestion;
        answerPods = this.questionUI.GetPossibleAnswers;
        ll.transform.DOScale(1, 0.5f).SetEase(Ease.OutQuad).From(0f).OnComplete(() =>
        {
            ll.GetComponent<LifeLine>().Ping();
        });
    }

    

    private void Update()
    {
        if (_state == LifeLineState.Start)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                answerPods[question.correctAnswer].ShowCorrect(true);
                _state = LifeLineState.Phase1;
                AudioManager.instance.PlaySound("c1-4");
            }
        }
        else if (_state == LifeLineState.Phase1)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                answerPods[question.correctAnswer].ShowCorrect(true);
                _state = LifeLineState.End;
                questionUI.FlipQuestion(GameManager.instance.GetSwitchQuestion());
                AudioManager.instance.StopSound("sw_start");
                AudioManager.instance.PlaySound("sw_flip");
                
                ll.GetComponent<LifeLine>().Ping();
                ll.transform.DOScale(0f, 0.3f).SetEase(Ease.InQuad).OnComplete(() =>
                {
                    if (!AudioManager.instance.IsMusicPlaying(GameManager.instance.GetBedName(GameManager.instance.currentQuestion)))
                    {
                        AudioManager.instance.PlayMusic(GameManager.instance.GetBedName(GameManager.instance.currentQuestion));
                    }
                    EndLifeline(true);
                });
            }
        }
    }
}
