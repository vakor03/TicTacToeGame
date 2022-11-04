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

    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    private int _moveCount;

    public void Awake()
    {
        SetGameControllerReferenceOnButtons();
        _playerSide = "X";
        gameOverPanel.SetActive(false);
        _moveCount = 0;
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
        _moveCount++;
        //Debug.Log("End Turn is not implemented"); 
        if (CheckForGameEnd(out bool isDraw))
        {
            GameOver(isDraw);
        }
        else
        {
            ChangeSides();
        }
    }

    private void GameOver(bool isDraw)
    {
        if (isDraw)
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = "It's a draw!";
            return;
        }
        
        for (var i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false;
        }

        gameOverPanel.SetActive(true);
        gameOverText.text = $"{_playerSide} Wins!";
    }

    private void ChangeSides()
    {
        _playerSide = (_playerSide == "X") ? "O" : "X";
    }

    private bool CheckForGameEnd(out bool isDraw)
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

        if (CheckRow(0) || CheckRow(1) || CheckRow(2) || CheckColumn(0) || CheckColumn(1) || CheckColumn(2) ||
            CheckDiagonals())
        {
            isDraw = false;
            return true;
        }

        if (_moveCount==9)
        {
            isDraw = true;
            return true;
        }

        isDraw = false;
        return false;
    }
}