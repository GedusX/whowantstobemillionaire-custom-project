using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ConfigScriptable", menuName = "ScriptableObjects/ConfigScriptable", order = 1)]
public class ConfigScriptable : ScriptableObject
{
    public string currency;
    public int currentQuestion = 0;
    public List<int> moneyTree = new List<int>();
    public List<int> mileStone = new List<int>();
    public List<Lifelines> startingLifelines = new List<Lifelines>();
    public List<Lifelines> mileStoneLifelines = new List<Lifelines>();
    public List<int> lifelineState = new List<int>(){0,0,0,-1,-1};
}
