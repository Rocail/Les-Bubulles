using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int difficultLevel;

    public void NewGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void SetDifficultLevel(int level)
    {
        difficultLevel = level;
    }

    public int GetDifficultLevel()
    {
        return (difficultLevel);
    }

}
