using System;
using NegamaxAlgorithms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        //_negamax = new Negamax();
        _negamax = new NegaScout();
       // _negamax = new NegamaxAlphaBetaPruning();
        _gameBoard = new Board();
    }

    public string PlayerSide => _playerSide;
    private string ComputerSide => _computerSide;

    private void SetGameControllerReferenceOnButtons()
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
            playerMove = true;
        }
        else
        {
            _computerSide = "X";
            SetPlayerColors(playerO, playerX);
            playerMove = false;
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
        playerMove = !playerMove;

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