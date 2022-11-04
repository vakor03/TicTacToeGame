using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Player
{
    public Image panel;
    public TMP_Text text;
    public Button button;
}

[Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public TMP_Text[] buttonList;
    private string _playerSide;

    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    private int _moveCount;

    public GameObject restartButton;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;


    public GameObject startInfo;

    public void Awake()
    {
        SetGameControllerReferenceOnButtons();
        //_playerSide = "X";
        gameOverPanel.SetActive(false);
        _moveCount = 0;
        restartButton.SetActive(false);
        //SetPlayerColors(playerX, playerO);
    }

    public string PlayerSide => _playerSide;

    public void SetGameControllerReferenceOnButtons()
    {
        for (var i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        _playerSide = startingSide;
        if (_playerSide=="X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
        
        StartGame();
    }

    public void EndTurn()
    {
        _moveCount++;
        //Debug.Log("End Turn is not implemented"); 
        if (CheckForGameEnd(out bool isDraw))
        {
            GameOver(isDraw);
            restartButton.SetActive(true);
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
            SetPlayerColorsInactive();
            return;
        }

        SetBoardInteractable(false);

        gameOverPanel.SetActive(true);
        gameOverText.text = $"{_playerSide} Wins!";
    }

    private void ChangeSides()
    {
        _playerSide = (_playerSide == "X") ? "O" : "X";

        switch (_playerSide)
        {
            case "X":
                SetPlayerColors(playerX, playerO);
                break;
            case "O":
                SetPlayerColors(playerO, playerX);
                break;
            default:
                throw new ArgumentException();
        }
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

        if (_moveCount == 9)
        {
            isDraw = true;
            return true;
        }

        isDraw = false;
        return false;
    }

    public void RestartGame()
    {
       // _playerSide = "X";
        _moveCount = 0;
        restartButton.SetActive(false);
        gameOverPanel.SetActive(false);

        for (var i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = String.Empty;
        }
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);
       // SetPlayerColors(playerX, playerO);

    }

    private void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    private void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    private void SetBoardInteractable(bool toggle)
    {
        for (var i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    private void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    private void SetPlayerColorsInactive()
    {
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
    }
}