using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] 
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
    public Sprite playerImage;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    //public Color textColor;
}

public class GameController : MonoBehaviour
{
    public GridSpace[] gridSpaceList;

    public Player playerX;
    public Player playerO;

    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    
    public Text gameOverText;
    
    public GameObject gameOverPanel;
    public GameObject restartButton;
    public GameObject startInfo;

    private string playerSide;
    private string playerName;

    private int moveCount;


    private void Awake()
    {
        SetGameControllerReferenceOnButtons();
        SetNewGame();
    }

    void SetGameControllerReferenceOnButtons()
    {
        for(int i = 0; i < gridSpaceList.Length; i++)
        {
            gridSpaceList[i].SetGameControllerReference(this);
        }
    }

    void SetNewGame()
    {        
        for (int i = 0; i < gridSpaceList.Length; i++)
        {
            gridSpaceList[i].ClearGridSpace();
        }

        moveCount = 0;

        SetPlayerButtons(true);
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayeColorsActive();
    }

    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;

        SwitchPlayerColors();
        StartGame();
    }

    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public Sprite GetPlayerSideImage()
    {
        if (playerSide == "X")
        {
            return playerX.playerImage;
        } 
        else 
        {
            return playerO.playerImage;
        }
    }

    public void EndTurn()
    {
        moveCount++;
        bool gameWon = false;

        for(int i = 0; i <= 6; i += 3) //horizontal rows
        {
            if(CheckRowSequence(i, i + 2, 1)){
                gameWon = true;
            }
        }

        for (int i = 0; i <= 2; i++) //vertical rows
        {
            if (CheckRowSequence(i, i + 6, 3))
            {
                gameWon = true;
            }
        }

        for (int i = 0; i <= 2; i += 2) //diagonal rows
        {       
            if (CheckRowSequence(i, 8 - i, 4 - i))
            {
                gameWon = true;
            }
        }

        GameOver(gameWon);

    }

    private bool CheckRowSequence (int startIndex, int endIndex, int shiftIndex)
    {

        for (int i = startIndex; i <= endIndex; i += shiftIndex)
        {
            if (gridSpaceList[i].text != playerSide)
            {
                return false;
            }
        }

        return true;
    }

    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
        playerName = (playerSide == "X") ? "Slayer" : "Vampire";
        SwitchPlayerColors();
    }

    void SwitchPlayerColors()
    {
        if(playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
        }else
        {
            SetPlayerColors(playerO, playerX);
        }
    }

    void GameOver(bool gameWon)
    {
        if (gameWon)
        {
            SetGameOver(playerName + " Wins!");
        }
        else if (moveCount >= gridSpaceList.Length)
        {
            SetGameOver("It's a draw!");
            SetPlayeColorsInactive();
        }
        else
        {
            ChangeSides();
        }
    }

    void SetGameOver (string panelText)
    {
        SetBoardInteractable(false);
        gameOverPanel.SetActive(true);
        gameOverText.text = panelText;
        restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        startInfo.SetActive(true);
        SetNewGame();
    }

    void SetBoardInteractable (bool toggle)
    {
        for (int i = 0; i < gridSpaceList.Length; i++)
        {
            gridSpaceList[i].button.interactable = toggle;
        }
    }

    void SetPlayerColors (Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
    }

    void SetPlayeColorsInactive ()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;  
        playerO.panel.color = inactivePlayerColor.panelColor; 
    }

    void SetPlayeColorsActive()
    {
        playerX.panel.color = activePlayerColor.panelColor;
        playerO.panel.color = activePlayerColor.panelColor;
    }

    void SetPlayerButtons (bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

}
