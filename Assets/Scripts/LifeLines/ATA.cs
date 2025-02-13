using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ATA : LifeLineProcess
{
    [SerializeField] private GameObject panel;
    [SerializeField] private List<Image> imgCols;
    [SerializeField] private List<TextMeshProUGUI> txtPers;
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;
    public override void StartLifeline(QuestionUI questionUI)
    {
        base.StartLifeline(questionUI);
        AudioManager.instance.StopALlMusic();
        AudioManager.instance.PlaySound("ata_start");

        
    }

    private IEnumerator ShowPanel()
    {
        List<int> op = new List<int>();
        _state = LifeLineState.Phase2;
        Question question = questionUI.GetCurrentQuestion;
        List<AnswerPod> answerPods = this.questionUI.GetPossibleAnswers;
        for (int t = 1; t <= 3; t++)
        {
            for (int i = 0; i < 10 * (4 - t); i++)
            {
                if (Random.Range(0f, 1f) <= 0.25 * t)
                {
                    op.Add(question.correctAnswer);
                }
                else
                {
                    int randomInd = Random.Range(0,answerPods.Count);
                    while (answerPods[randomInd].gameObject.activeSelf == false || randomInd == question.correctAnswer)
                    {
                        randomInd = Random.Range(0,answerPods.Count);
                    }
                    op.Add(randomInd);
                }
            }
        }
        
        List<float> percents = new List<float>();
        for (int i = 0; i < 4; i++)
        {
            percents.Add(op.Where((x) => x == i).Count() * 1.0f / op.Count * 1.0f);
        }
        for (int i = 0; i < 4; i++)
        {
            imgCols[i].GetComponent<RectTransform>().sizeDelta = new Vector2(90f, minHeight);
            txtPers[i].text = Mathf.RoundToInt(percents[i] * 100f).ToString() + "%";
        }
        panel.SetActive(true);
        panel.transform.DOLocalRotate(Vector3.zero, 0.5f).From(new Vector3(0, -80, 0));
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.StopSound("ata_during");
        AudioManager.instance.PlaySound("ata_end");
        for (int i = 0; i < 4; i++)
        {
            imgCols[i].GetComponent<RectTransform>().DOSizeDelta(new Vector2(90f, (maxHeight - minHeight) * percents[i] * 1.5f + minHeight), .4f).SetEase(Ease.InOutQuad).SetDelay(i * 0.1f);
            imgCols[i].GetComponent<RectTransform>().DOSizeDelta(new Vector2(90f, (maxHeight - minHeight) * percents[i] + minHeight), .4f).SetEase(Ease.InOutQuad).SetDelay(i * 0.1f + 0.4f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 4; i++)
        {
            txtPers[i].DOFade(1f, 0.5f).SetEase(Ease.InOutQuad).From(0f);
        }

        _state = LifeLineState.End;
        if (!AudioManager.instance.IsMusicPlaying(GameManager.instance.GetBedName(GameManager.instance.currentQuestion)))
        {
            AudioManager.instance.PlayMusic(GameManager.instance.GetBedName(GameManager.instance.currentQuestion));
        }

    }
    void Update()
    {
        if (_state == LifeLineState.Start)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _state = LifeLineState.Phase1;
                AudioManager.instance.StopSound("ata_start");
                AudioManager.instance.PlaySound("ata_during");
            }
        }
        else if (_state == LifeLineState.Phase1)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(ShowPanel());
            }
        }
        else if (_state == LifeLineState.End)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                panel.transform.DOLocalMoveX(2000, 0.5f).OnComplete(() =>
                {
                    EndLifeline();
                }).SetEase(Ease.InQuad);
            }
        }
    }
}
