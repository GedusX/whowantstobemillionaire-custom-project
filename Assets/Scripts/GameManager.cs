using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum State
{
    Idle,
    RuleExplain,
    InGame,
    MidGame,
    LifeLine,
    GameOver
}
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public QuestionScriptable questionScript;
    public ConfigScriptable configScript;
    
    private List<GameObject> objectStack = new List<GameObject>();
    
    [SerializeField] private RectTransform canvas;

    [SerializeField] private GameObject question;
    [SerializeField] private GameObject moneyTree;
    [SerializeField] private GameObject gameOver;
    
    public State currentState = State.Idle;
    public int achievedQuestion { get; private set; } = -1;
    public int currentQuestion { get; private set; }= 0;
    public int playFor { get; private set; } = 0;
    public int lost { get; private set; } = 0;
    public int walkaway { get; private set; } = 0;

    public int finalValue = 0;
    
    public static UnityEvent<int> OnCorrect = new UnityEvent<int>();
    public static UnityEvent<int> OnIncorrect = new UnityEvent<int>();
    public static UnityEvent<int> OnWalkAway = new UnityEvent<int>();

    public Dictionary<Lifelines, int> lifelines = new Dictionary<Lifelines, int>();

    public void PopStack(GameObject obj)
    {
        if (objectStack.Contains(obj))
        {
            objectStack.Remove(obj);
            Destroy(obj);
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        OnCorrect.AddListener((i) => LevelUp(i));
        OnIncorrect.AddListener(i => LevelOver());
        OnWalkAway.AddListener((i) => SaveMoney());
    }

    public List<int> GetMilestone()
    {
        List<int> achievedMilestones = configScript.mileStone.Where((x) => x <= achievedQuestion).ToList();
        return achievedMilestones;
    }

    void Start()
    {
        StartGame();
    }

    public void LevelUp(int id)
    {
        if (questionScript.questions.Count - 1 == id)
        {
            
            AudioManager.instance.StopALlSound();
            AudioManager.instance.StopALlMusic();
            AudioManager.instance.PlaySound(GetCorrect(currentQuestion));
            finalValue = playFor;
            return;
        }
        achievedQuestion = id;
        currentQuestion = id + 1;
        
        playFor = configScript.moneyTree[currentQuestion];
        currentState = State.InGame;

        walkaway = (achievedQuestion >= 0) ? configScript.moneyTree[achievedQuestion] : 0;
        List<int> achievedMilestones = configScript.mileStone.Where((x) => x <= achievedQuestion).ToList();
        if (achievedMilestones.Count > 0)
        {
            lost = configScript.moneyTree[achievedMilestones.Last()];
            AudioManager.instance.StopALlSound();
            AudioManager.instance.StopALlMusic();
        }
        else
        {
            lost = 0;
        }
        AudioManager.instance.PlaySound(GetCorrect(achievedQuestion));

    }

    public void LevelOver()
    {
        finalValue = lost;
        AudioManager.instance.StopALlSound();
        AudioManager.instance.StopALlMusic();
        AudioManager.instance.PlaySound(GetIncorrect(currentQuestion));
    }

    public void SaveMoney()
    {
        finalValue = walkaway;
        AudioManager.instance.StopALlSound();
        AudioManager.instance.StopALlMusic();
    }

    public void StartGame()
    {
        //currentState = State.InGame;
        currentQuestion = configScript.currentQuestion;
        achievedQuestion = currentQuestion - 1;
        playFor = configScript.moneyTree[currentQuestion];
        walkaway = (achievedQuestion >= 0) ? configScript.moneyTree[achievedQuestion] : 0;
        List<int> achievedMilestones = configScript.mileStone.Where((x) => x <= achievedQuestion).ToList();
        if (achievedMilestones.Count > 0)
        {
            lost = configScript.moneyTree[achievedMilestones.Last()];
        }
        else
        {
            lost = 0;
        }

        if (configScript.lifelineState.Count == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                lifelines[(Lifelines) i] =  configScript.startingLifelines.Contains((Lifelines) i) ? 0 : -1;
            }
        }
        else
        {
            for (int i = 0; i < configScript.lifelineState.Count; i++)
            {
                lifelines[(Lifelines) i] =  configScript.lifelineState[i]; 
            }
        }
        
    }

    public Question GetQuestion(int index)
    {
        Question q = questionScript.questions[index];
        q.questionIndex = index;
        q.answerFor = playFor;
        return q;
    }
    
    public Question GetSwitchQuestion()
    {
        Question q = questionScript.switchQuestions[Random.Range(0, questionScript.switchQuestions.Count)];
        return q;
    }
    
    public void StartQuestion()
    {
        playFor = configScript.moneyTree[currentQuestion];
        currentState = State.InGame;

        walkaway = (achievedQuestion >= 0) ? configScript.moneyTree[achievedQuestion] : 0;
        List<int> achievedMilestones = configScript.mileStone.Where((x) => x <= achievedQuestion).ToList();
        if (achievedMilestones.Count > 0)
        {
            lost = configScript.moneyTree[achievedMilestones.Last()];
        }
        else
        {
            lost = 0;
        }
    }

    public string GetBedName(int i)
    {
        List<int> achievedMilestones = configScript.mileStone.Where((x) => x <= achievedQuestion).ToList();
        if (achievedMilestones.Count > 0)
        {
            return "bed_q" + (i+1).ToString(); 
        }
        else
        {
            return "bed_q1-5";
        }
    }

    public string GetLightDownName(int i)
    {
        List<int> achievedMilestones = configScript.mileStone.Where((x) => x <= achievedQuestion).ToList();
        if (achievedMilestones.Count >= 2)
        {
            return $"l{i - 5 + 1}-{i - 5 + 1 + 5}";
        }
        else if (achievedMilestones.Count >= 1)
        {
            return $"l{i+ 1}-{i+ 1 + 5}";
        }
        else
        {
            return "l1-5";
        }
    }

    public string GetCorrect(int i)
    {
        List<int> achievedMilestones = configScript.mileStone.Where((x) => x <= achievedQuestion).ToList();
        if (achievedMilestones.Count > 0)
        {
            return $"c{i + 1}";
        }
        else
        {
            if (configScript.mileStone[0] == i)
                return "c5";
            else
            {
                return "c1-4";
            }
        }
    }
    
    public string GetIncorrect(int i)
    {
        List<int> achievedMilestones = configScript.mileStone.Where((x) => x <= achievedQuestion).ToList();
        if (achievedMilestones.Count > 0)
        {
            return $"w{i + 1}";
        }
        else
        {
            return "w1-5";

        }
    }


    public string GetFinal(int i)
    {
        List<int> achievedMilestones = configScript.mileStone.Where((x) => x <= achievedQuestion).ToList();
        if (achievedMilestones.Count >= 2)
        {
            return $"faq{i - 5 + 1}-{i - 5 + 1 + 5}";
        }
        else if (achievedMilestones.Count >= 1)
        {
            return $"faq{i+ 1}-{i+ 1 + 5}";
        }

        else
        {
            return "-----------";

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.Idle || currentState == State.RuleExplain)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (objectStack.Count == 0 || !objectStack[0].GetComponent<MoneyTree>())
                {
                    currentState = State.RuleExplain;
                    objectStack.Insert(0, Instantiate(moneyTree, canvas));
                    AudioManager.instance.PlayMusic("standby");
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (objectStack.Count == 0 || !objectStack[0].GetComponent<MoneyTree>())
                {
                    currentState = State.InGame;
                    StartGame();
                    AudioManager.instance.StopALlSound();
                    AudioManager.instance.StopALlMusic();
                    AudioManager.instance.PlaySound(GetLightDownName(currentQuestion));

                }
            }
        }
        else if (currentState == State.InGame)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (objectStack.Count == 0)
                {
                    objectStack.Insert(0, Instantiate(moneyTree, canvas));
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (objectStack.Count == 0)
                {
                    AudioManager.instance.StopSound(GetLightDownName(currentQuestion));
                    objectStack.Insert(0, Instantiate(question, canvas));
                    objectStack[0].GetComponent<QuestionUI>().QuestionShow(GetQuestion(currentQuestion));
                    if (!AudioManager.instance.IsMusicPlaying(GetBedName(currentQuestion)))
                    {
                        AudioManager.instance.PlayMusic(GetBedName(currentQuestion));
                    }
                }
            }
        }
        else if (currentState == State.MidGame)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (objectStack.Count == 0)
                {
                    objectStack.Insert(0, Instantiate(moneyTree, canvas));
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (objectStack.Count == 0)
                {
                    StartQuestion();
                    AudioManager.instance.StopALlSound();
                    AudioManager.instance.StopALlMusic();
                    AudioManager.instance.PlaySound(GetLightDownName(currentQuestion));

                }
            }
        }
        else if (currentState == State.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (objectStack.Count == 0)
                {
                    objectStack.Insert(0, Instantiate(gameOver, canvas));
                    AudioManager.instance.PlaySound("total_prize");
                }
            }
        }
    }
}
