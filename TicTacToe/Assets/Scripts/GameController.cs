using System;
using NegamaxAlgorithms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Negamax = NegamaxAlgorithms.Negamax;

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

    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    public GameObject restartButton;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;


    public GameObject startInfo;

    private string _playerSide;
    private string _computerSide;
    public bool playerMove;
    public float delay;

    private INegamax _negamax;

    public void Awake()
    {
        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        playerMove = true;
        //_negamax = new Negamax();
        _negamax = new NegamaxAlphaBetaPruning();
        _gameBoard = new Board();
    }

    public string PlayerSide => _playerSide;
    public string ComputerSide => _computerSide;

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
        if (_playerSide == "X")
        {
            _computerSide = "O";
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            _computerSide = "X";
            SetPlayerColors(playerO, playerX);
        }

        _negamax.ComputerSide = _computerSide;
        _negamax.PlayerSide = _playerSide;
        StartGame();
    }

    public void EndTurn()
    {
        UpdateGameBoard();
        if (CheckForGameEnd(out bool isDraw, out string side))
        {
            GameOver(isDraw, side);
            restartButton.SetActive(true);
        }
        else
        {
            ChangeSides();
        }
    }

    private void UpdateGameBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _gameBoard[i, j] = buttonList[i * 3 + j].text;
            }
        }
    }

    private void GameOver(bool isDraw, string winningSide)
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
        gameOverText.text = $"{winningSide} Wins!";
    }

    private void ChangeSides()
    {
        playerMove = playerMove ? false : true;

        switch (playerMove)
        {
            case true:
                SetPlayerColors(playerX, playerO);
                break;
            case false:
                SetPlayerColors(playerO, playerX);
                break;
        }
    }

    private Board _gameBoard;

    private bool CheckForGameEnd(out bool isDraw, out string side)
    {
        // side = String.Empty;
        // bool CheckRow(int rowNum, string playerSide)
        // {
        //    
        //     return buttonList[rowNum * 3].text == playerSide && buttonList[rowNum * 3 + 1].text == playerSide &&
        //            buttonList[rowNum * 3 + 2].text == playerSide;
        // }
        //
        // bool CheckColumn(int colNum, string playerSide)
        // {
        //     return buttonList[colNum].text == playerSide && buttonList[colNum + 3].text == playerSide &&
        //            buttonList[colNum + 6].text == playerSide;
        // }
        //
        // bool CheckDiagonals(string playerSide)
        // {
        //     return buttonList[0].text == playerSide && buttonList[4].text == playerSide &&
        //            buttonList[8].text == playerSide ||
        //            buttonList[2].text == playerSide && buttonList[4].text == playerSide &&
        //            buttonList[6].text == playerSide;
        // }
        //
        // if (CheckRow(0, _playerSide) || CheckRow(1, _playerSide) || CheckRow(2, _playerSide) || CheckColumn(0, _playerSide) || CheckColumn(1, _playerSide) || CheckColumn(2, _playerSide) ||
        //     CheckDiagonals(_playerSide))
        // {
        //     isDraw = false;
        //     side = _playerSide;
        //     return true;
        // }
        //
        // if (CheckRow(0, _computerSide) || CheckRow(1,_computerSide) || CheckRow(2,_computerSide) || CheckColumn(0,_computerSide) || CheckColumn(1,_computerSide) || CheckColumn(2,_computerSide) ||
        //     CheckDiagonals(_computerSide))
        // {
        //     isDraw = false;
        //     side = _computerSide;
        //     return true;
        // }
        //
        // if (_moveCount == 9)
        // {
        //     isDraw = true;
        //     return true;
        // }
        //
        // isDraw = false;
        // return false;
        isDraw = _gameBoard.State == Board.GameState.Draw;
        _gameBoard.CheckForWin(out side);
        return _gameBoard.State != Board.GameState.NotFinished;
    }

    public void RestartGame()
    {
        restartButton.SetActive(false);
        gameOverPanel.SetActive(false);

        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);
        playerMove = true;
        delay = 0;

        for (var i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = String.Empty;
        }
    }

    private void StartGame()
    {
        _gameBoard.Clear();
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

    private void Update()
    {
        if (playerMove == false && _gameBoard.State==Board.GameState.NotFinished)
        {
            delay += Time.deltaTime;
            if (delay >= 1)
            {
                var bestMove = _negamax.FindBestTurn(buttonList);
                int index = bestMove.Row * 3 + bestMove.Col;
                buttonList[index].text = ComputerSide;
                buttonList[index].GetComponentInParent<Button>().interactable = false;
                EndTurn();
                delay = 0;
            }
        }
    }
}