using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FF : LifeLineProcess
{
    // Start is called before the first frame update
    public override void StartLifeline(QuestionUI questionUI)
    {
        base.StartLifeline(questionUI);
        AudioManager.instance.PlaySoundOneShot("5050");
        Question question = questionUI.GetCurrentQuestion;
        List<AnswerPod> answerPods = this.questionUI.GetPossibleAnswers;
        List<int> wrongAnswers = new List<int>();
        for (int i = 0; i < 2; i++)
        {
            int randomInd = Random.Range(0,answerPods.Count);
            while (answerPods[randomInd].gameObject.activeSelf == false || wrongAnswers.Contains(randomInd) || question.correctAnswer == randomInd)
            {
                randomInd = Random.Range(0,answerPods.Count);
            }
            wrongAnswers?.Add(randomInd);
            answerPods[randomInd].HideAnswerPod(false);
            EndLifeline();
        }
        
    }
}
