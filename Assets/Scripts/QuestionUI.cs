using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public enum Lifelines
{
    FF = 0,
    AG = 1,
    ATA = 2,
    DD = 3,
    SW = 4
}

public enum QState
{
    Start,
    During,
    LifeLine,
    FinalAnswer,
    Correct,
    Incorrect,
    Walk,
    Walkend,
    Reward
}
[Serializable]
public class Question
{
    
    
    
    public string question;
    public List<string> answers;
    public int correctAnswer;
    public int answerFor;
    public int questionIndex;
    public Question(int id, string question, List<string> answers, int correctAnswer, int prize)
    {
        this.question = question;
        this.answers = answers;
        this.correctAnswer = correctAnswer;
        this.answerFor = prize;
        this.questionIndex = id;
    }
}
public class QuestionUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image questionImage;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image answerImage1;
    [SerializeField] private Image answerImage2;
    [SerializeField] private List<AnswerPod> possibleAnswers;
    
    
    [SerializeField] private GameObject prizeUI;
    [SerializeField] private TextMeshProUGUI txtPrize;
    [SerializeField] private Image rotateImg;
    
    [SerializeField] private KeyCode[] ansKey = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
    
    [SerializeField] private Question currentQuestion;
    
    
    private QState state;

    public AnswerPod isChoosing { get; private set; } = null;
    public int answerId { get; private set; } = -1;

    public LifeLineProcess OnGoingLifeline;


    [SerializeField] private List<GameObject> prefabLL;
    private KeyCode[] llKeyCodes = new[] { KeyCode.F, KeyCode.G, KeyCode.A, KeyCode.D, KeyCode.S };
    public TextMeshProUGUI prizePreviewTxt;
    public LLPanel llPanel;
    


    public Question GetCurrentQuestion => currentQuestion;

    public List<AnswerPod> GetPossibleAnswers => possibleAnswers;
    
    

    public void QuestionShow(Question question)
    {
        currentQuestion = question;
        questionText.text = question.question;
        txtPrize.text = question.answerFor.ToString("N0");
        state = QState.Start;

        questionImage.transform.DOLocalRotate(new Vector3(0,0,0), 0.6f).From(new Vector3(-80,0,0)).SetEase(Ease.OutQuad).OnStart(() => questionImage.gameObject.SetActive(true));
        answerImage1.transform.DOLocalRotate(new Vector3(0,0,0), 0.6f).From(new Vector3(-80,0,0)).SetEase(Ease.OutQuart).SetDelay(0.6f).OnStart(() => answerImage1.gameObject.SetActive(true));
        answerImage2.transform.DOLocalRotate(new Vector3(0,0,0), 0.6f).From(new Vector3(-80,0,0)).SetEase(Ease.OutQuart).SetDelay(0.9f).OnStart(() => answerImage2.gameObject.SetActive(true));
    }

    public void SetState(QState sstate)
    {
        this.state = sstate;
    }

    public void FlipQuestion(Question question)
    {
        currentQuestion = question;
        questionImage.transform.DOLocalRotate(new Vector3(360, 0, 0), 1.6f, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad);
        float temp1 = 0;
        DOTween.To(() => temp1, x => temp1 = x, 360, 1.6f).SetEase(Ease.InOutQuad).OnUpdate(
            () =>
            {
                if (temp1 > 270)
                {
                    questionText.gameObject.SetActive(true);
                    questionText.text = question.question;
                }
                else if (temp1 > 90)
                {
                    questionText.gameObject.SetActive(false);
                }
            }
            );

        
        answerImage1.transform.DOLocalRotate(new Vector3(360,0,0), 1.5f, RotateMode.FastBeyond360).SetEase(Ease.InOutQuart).SetDelay(0.3f).OnUpdate(
            () =>
            {
                if (answerImage1.transform.localEulerAngles.z > 90 && !prizeUI.activeSelf)
                {
                    possibleAnswers[0].gameObject.SetActive(false);
                    possibleAnswers[1].gameObject.SetActive(false);
                    possibleAnswers[0].ResetPod();
                    possibleAnswers[1].ResetPod();
                }
            });
        
        answerImage2.transform.DOLocalRotate(new Vector3(360,0,0), 1.5f, RotateMode.FastBeyond360).SetEase(Ease.InOutQuart).SetDelay(0.45f).OnUpdate(
            () =>
            {
                if (answerImage2.transform.localEulerAngles.z > 90 && !prizeUI.activeSelf)
                {
                    possibleAnswers[2].gameObject.SetActive(false);
                    possibleAnswers[3].gameObject.SetActive(false);
                    possibleAnswers[2].ResetPod();
                    possibleAnswers[3].ResetPod();
                }
            });
    }

    public void StartLifeline(Lifelines lifeline)
    {
        state = QState.LifeLine;
        OnGoingLifeline = Instantiate(prefabLL[(int)lifeline], transform).GetComponent<LifeLineProcess>();
        OnGoingLifeline.StartLifeline(this);
    }

    public void EndLifeline()
    {
        if (OnGoingLifeline != null)
        {
            Destroy(OnGoingLifeline.gameObject);
        }
        OnGoingLifeline = null;
    }


    public void ShowReward()
    {
        Vector3 oldPos = questionImage.transform.localPosition;
        answerImage1.transform.DOLocalRotate(new Vector3(100, 0, 0), 0.5f).OnComplete(() => answerImage1.gameObject.SetActive(false));
        answerImage2.transform.DOLocalRotate(new Vector3(100, 0, 0), 0.5f).OnComplete(() => answerImage2.gameObject.SetActive(false));
        
        
        questionImage.transform.DOLocalMoveY(oldPos.y - 200f, 1.6f).SetEase(Ease.InOutQuad);
        questionImage.transform.DOLocalRotate(new Vector3(180, 0, 0), 1.6f).OnUpdate(() =>
        {
            if (questionImage.transform.localEulerAngles.z > 90 && !prizeUI.activeSelf)
            {
                questionText.gameObject.SetActive(false);
                prizeUI.SetActive(true);
            }
        });


    }
    

    // Update is called once per frame
    void Update()
    {
        if (state == QState.Start)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                for (int i = 0; i < possibleAnswers.Count; i++)
                {
                    if (!possibleAnswers[i].gameObject.activeSelf)
                    {
                        possibleAnswers[i].ShowAnswerPod(currentQuestion.answers[i]);
                        if (i == possibleAnswers.Count - 1)
                        {
                            state = QState.During;
                        }
                        else
                        {
                            break;
                        }
                    }

                }
            }
        }
        else if (state == QState.During)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (llPanel.gameObject.activeSelf)
                {
                    llPanel.HidePanel();
                }
                else
                {
                    llPanel.ShowPanel();
                }
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                prizePreviewTxt.text = GameManager.instance.playFor.ToString("N0");
                if (prizePreviewTxt.transform.parent.gameObject.activeSelf)
                {
                    prizePreviewTxt.transform.parent.DOLocalRotate(Vector3.left * 90, 0.5f)
                        .OnComplete(() => prizePreviewTxt.transform.parent.gameObject.SetActive(false));
                }
                else
                {
                    prizePreviewTxt.transform.parent.DOLocalRotate(Vector3.zero, 0.5f).From(Vector3.left * 90)
                        .OnStart(() => prizePreviewTxt.transform.parent.gameObject.SetActive(true));
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                state = QState.Walk;
                GameManager.OnWalkAway.Invoke(currentQuestion.questionIndex);
            }
            else
            {
                for (int i = 0; i < ansKey.Length; i++)
                {
                    if (Input.GetKeyDown(ansKey[i]) && possibleAnswers[i].gameObject.activeSelf)
                    {
                        possibleAnswers[i].ChooseAnswerPod();
                        isChoosing = possibleAnswers[i];
                        answerId = i;
                        state = QState.FinalAnswer;
                        if (GameManager.instance.GetMilestone().Count == 0)
                        {
                            
                        }
                        else
                        {
                            //AudioManager.instance.StopALlSound();
                            AudioManager.instance.StopALlMusic();
                        }
                        AudioManager.instance.PlaySound(GameManager.instance.GetFinal(GameManager.instance.currentQuestion));
                    }
                }
                for (int i = 0; i < llKeyCodes.Length; i++)
                {
                    if (Input.GetKeyDown(llKeyCodes[i]))
                    {
                        if (GameManager.instance.lifelines[(Lifelines)i] == 0)
                        {
                            StartLifeline((Lifelines) i);
                        }
                    }
                }
            }
            
        }
        else if (state == QState.FinalAnswer)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentQuestion.correctAnswer == answerId)
                {
                    state = QState.Correct;
                    GameManager.OnCorrect.Invoke(GameManager.instance.currentQuestion);
                }
                else
                {
                    state = QState.Incorrect;
                    GameManager.OnIncorrect.Invoke(GameManager.instance.currentQuestion);
                }
                possibleAnswers[currentQuestion.correctAnswer].ShowCorrect(true);
            }
        }
        else if (state == QState.Correct)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ShowReward();
                state = QState.Reward;
            }
        }
        else if (state == QState.Reward)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GameManager.instance.PopStack(gameObject);
                if (GameManager.instance.lost <= 0)
                {
                    GameManager.instance.currentState = State.InGame;
                }
                else if (GameManager.instance.questionScript.questions.Count - 1 == currentQuestion.questionIndex)
                {
                    GameManager.instance.currentState = State.GameOver;
                }
                else
                {
                    GameManager.instance.currentState = State.MidGame;
                }
            }
        }
        else if (state == QState.Incorrect)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GameManager.instance.PopStack(gameObject);
                GameManager.instance.currentState = State.GameOver;
            }
        }
        else if (state == QState.Walk)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                possibleAnswers[currentQuestion.correctAnswer].ShowCorrect(true);
                state = QState.Walkend;

            }
        }
        else if (state == QState.Walkend)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GameManager.instance.PopStack(gameObject);
                GameManager.instance.currentState = State.GameOver;
            }
        }
    }
}
