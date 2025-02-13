using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class moneyCell : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI txtID;
    [SerializeField] private TextMeshProUGUI txtAmount;
    [SerializeField] private Image imgPin;
    [SerializeField] private Sprite[] choosePin;

    private int id;
    private bool isMilestone;

    public void Setup(int id, int currentQuestion, int amount, bool isMilestone)
    {
        this.id = id;
        txtID.text = (id+1).ToString();
        txtAmount.text = amount.ToString("N0");
        this.isMilestone = isMilestone;
        if (isMilestone)
        {
            txtID.color = Color.white;
            txtAmount.color = Color.white;
        }
        else
        {
            txtID.color = new Color(1, 0.5f,0 , 1);
            txtAmount.color = new Color(1, 0.5f,0 , 1);
        }

        if (id < currentQuestion)
        {
            //imgPin.sprite = choosePin[Convert.ToInt16((id == currentQuestion))];
            imgPin.color = Color.white;
        }
        else if (id == currentQuestion)
        {
            //imgPin.sprite = choosePin[Convert.ToInt16((id == currentQuestion))];
            imgPin.color = Color.white;
            if (this.isMilestone)
            {
                txtID.color = Color.white;
                txtAmount.color = Color.white;
            }
            else
            {
                txtAmount.color = Color.black;
                txtID.color = Color.black;
            }
            
        }
        else
        {
            imgPin.color = Color.clear;
        }
    }
    
}
