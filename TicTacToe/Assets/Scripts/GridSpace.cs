using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public TMP_Text buttonText;

    public GameController _gameController;

    public void SetSpace()
    {
        buttonText.text = _gameController.PlayerSide;
        button.interactable = false;
        _gameController.EndTurn();
    }

    public void SetGameControllerReference(GameController gameController)
    {
        _gameController = gameController;
    }
}