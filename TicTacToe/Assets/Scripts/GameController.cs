using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TMP_Text[] buttonList;

    public void Awake()
    {
        SetGameControllerReferenceOnButtons();
    }

    public string PlayerSide => "?";

    public void SetGameControllerReferenceOnButtons()
    {
        for (var i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void EndTurn()
    {
        Debug.Log("End Turn is not implemented");
    }
}