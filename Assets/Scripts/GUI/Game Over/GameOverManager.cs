using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void Restart()
    {
        GameManager.Main.StartGameLoop();
    }

    public void Title()
    {
        Destroy(GameManager.Main.gameObject);
        Destroy(AudioManager.Main.gameObject);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
