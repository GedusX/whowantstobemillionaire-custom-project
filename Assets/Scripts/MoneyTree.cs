using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTree : MonoBehaviour
{
    [SerializeField] private List<moneyCell> moneyCells = new List<moneyCell>();
    [SerializeField] private moneyCell moneyCellPrefab;
    [SerializeField] private GameObject slider;
    [SerializeField] private RectTransform rectTree;
    [SerializeField] private List<LifeLine> lifes = new List<LifeLine>();

    private List<int> rewards = new List<int>();
    private List<int> miles = new List<int>();

    private KeyCode[] ll = new[] { KeyCode.F, KeyCode.G, KeyCode.A, KeyCode.D, KeyCode.S };
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        rewards = GameManager.instance.configScript.moneyTree;
        miles = GameManager.instance.configScript.mileStone;
        for (int i = 0; i < rewards.Count; i++)
        {
            moneyCells.Add(Instantiate(moneyCellPrefab.gameObject, rectTree).GetComponent<moneyCell>());
            moneyCells[i].Setup(i, GameManager.instance.achievedQuestion, rewards[i], miles.Contains(i));
            moneyCells[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < lifes.Count; i++)
        {
            lifes[i].SetState(GameManager.instance.lifelines[(Lifelines)i]);
        }

        
    }

    public void SetMoney(int id)
    {
        for (int i = 0; i < moneyCells.Count; i++)
        {
            moneyCells[i].Setup(i, id, rewards[i], miles.Contains(i));
        }
    }
    private IEnumerator ruleExplain()
    {
        for (int i = 0; i < moneyCells.Count; i++)
        {
            Canvas.ForceUpdateCanvases();
            SetMoney(i);
            slider.gameObject.SetActive(true);
            slider.transform.position = moneyCells[i].transform.position + Vector3.left * 1.75f;
            yield return new WaitForSeconds(0.25f);
        }
    }

    void Update()
    {
        if (GameManager.instance.currentState == State.RuleExplain)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (GameManager.instance.achievedQuestion == -1)
                {
                    StartCoroutine(ruleExplain());
                }
            }
            for (int i = 0; i < ll.Length; i++)
            {
                if (Input.GetKeyDown(ll[i]))
                {
                    if (lifes[i].gameObject.activeSelf)
                    {
                        lifes[i].Ping();
                        AudioManager.instance.PlaySoundOneShot($"ping{i+1}");
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < ll.Length; i++)
            {
                if (Input.GetKeyDown(ll[i]))
                {
                    int id1 = GameManager.instance.configScript.mileStoneLifelines.IndexOf((Lifelines)i);
                    int id2 = GameManager.instance.configScript.mileStone.IndexOf(GameManager.instance.achievedQuestion);
                    if (id2 != -1 && id2 != -1 && GameManager.instance.lifelines[(Lifelines) i] == -1 && id1 == id2)
                    {
                        lifes[i].SetState(0);
                        GameManager.instance.lifelines[(Lifelines)i] = 0;
                        lifes[i].Ping();
                        AudioManager.instance.PlaySoundOneShot($"ping{i}");
                    }
                }
            }
        }
        
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameManager.instance.PopStack(gameObject);
        }
        
        if (GameManager.instance.achievedQuestion != -1)
        {
            slider.gameObject.SetActive(true);
            slider.transform.position = moneyCells[GameManager.instance.achievedQuestion].transform.position +
                                        Vector3.left * 1.75f;
        }
        else
        {
            //slider.gameObject.SetActive(false);
        }

        
    }
}
