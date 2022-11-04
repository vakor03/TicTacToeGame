using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;

    public TMP_Text buttonText;

    public string playerSide;

    public void SetSpace()
    {
        buttonText.text = playerSide;
        button.interactable = false;
    }
}
