using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TMP_Text[] buttonList;
    private string _playerSide;

    public void Awake()
    {
        SetGameControllerReferenceOnButtons();
        _playerSide = "X";
    }

    public string PlayerSide => _playerSide;

    public void SetGameControllerReferenceOnButtons()
    {
        for (var i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void EndTurn()
    {
        //Debug.Log("End Turn is not implemented");
        if (CheckForWin())
        {
            GameOver();
        }
    }
    private bool CheckForWin()
    {
        bool CheckRow(int rowNum)
        {
            return buttonList[rowNum * 3].text == _playerSide && buttonList[rowNum * 3 + 1].text == _playerSide &&
                   buttonList[rowNum * 3 + 2].text == _playerSide;
        }

        bool CheckColumn(int colNum)
        {
            return buttonList[colNum].text == _playerSide && buttonList[colNum + 3].text == _playerSide &&
                   buttonList[colNum + 6].text == _playerSide;
        }

        bool CheckDiagonals()
        {
            return buttonList[0].text == _playerSide && buttonList[4].text == _playerSide &&
                   buttonList[8].text == _playerSide ||
                   buttonList[2].text == _playerSide && buttonList[4].text == _playerSide &&
                   buttonList[6].text == _playerSide;
        }

        return CheckRow(0) || CheckRow(1) || CheckRow(2) || CheckColumn(0) || CheckColumn(1) || CheckColumn(2) || CheckDiagonals();
    }

    private void GameOver()
    {
        for (var i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false;
        }
    }
}