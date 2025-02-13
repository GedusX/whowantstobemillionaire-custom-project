using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionScriptable", menuName = "ScriptableObjects/QuestionScriptable", order = 1)]
public class QuestionScriptable : ScriptableObject
{
    public List<Question> questions = new List<Question>();
    public List<Question> switchQuestions = new List<Question>();
}
