using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeLineProcess : MonoBehaviour
{
    public enum LifeLineState
    {
        Start,
        Phase1,
        Phase2,
        Phase3,
        Phase4,
        Phase5,
        End,
    }
    public QuestionUI questionUI;
    protected bool started = false;
    public Lifelines _lifelineType;
    
    protected LifeLineState _state = LifeLineState.Start;


    public QState endState;
    public virtual void StartLifeline(QuestionUI questionUI)
    {
        this.questionUI = questionUI;
        started = true;
        _state = LifeLineState.Start;
    }

    public void EndLifeline(bool isSetState = true)
    {
        if (isSetState)
        {
            questionUI.SetState(endState);
        }
        GameManager.instance.lifelines[_lifelineType] = 1;
        questionUI.EndLifeline();
    }
}
