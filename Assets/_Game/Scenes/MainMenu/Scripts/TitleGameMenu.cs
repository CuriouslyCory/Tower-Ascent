using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleGameMenu : MonoBehaviour
{
    [SerializeField]
    GameState gameState;

    public void PlayGame()
    {
        SceneManager.LoadScene("Tavern");
        gameState.inventory.gold = 10;
        gameState.playerHealth = 12;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
