using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TTPW : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtPrize;


    private void Start()
    {
        txtPrize.text = GameManager.instance.finalValue.ToString("N0");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameManager.instance.PopStack(gameObject);
        }
    }
}
